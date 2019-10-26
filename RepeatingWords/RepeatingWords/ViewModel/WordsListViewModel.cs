using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Heleprs;
using RepeatingWords.Model;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class WordsListViewModel : BaseListViewModel
    {
       
        public WordsListViewModel(INavigationService navigationServcie, IDialogService dialogService, IUnitOfWork unitOfWork, IImportFile importFile) : base(navigationServcie, dialogService, unitOfWork, importFile)
        {
            AddWordCommand = new Command(async()=> { await NavigationService.NavigateToAsync<CreateWordViewModel>(_dictionary); SetUnVisibleFloatingMenu(); });
            RepeatingWordsCommand = new Command(async()=> { await NavigationService.NavigateToAsync<RepeatingWordsViewModel>(_dictionary); });
            SetUnVisibleFloatingMenu();
        }

        protected override async Task ImportFile()
        {
            try
            {
                if (!await _importFile.PickFile(_dictionary.Id))
                   throw new Exception("Error import words from file");
                await InitializeAsync(_dictionary);
            }
            catch (Exception er)
            {
                DialogService.ShowToast(Resource.ErrorImport);
                Log.Logger.Error(er);
            }
        }

        private Dictionary _dictionary;
        private string _dictionaryName;
        public string DictionaryName { get => _dictionaryName;
            set {
                _dictionaryName = value;
                if (_dictionaryName.EndsWith(Constants.NAME_DB_FOR_CONTINUE))
                    _dictionaryName = _dictionaryName.Replace(Constants.NAME_DB_FOR_CONTINUE, "");
                if (_dictionaryName.EndsWith(Resource.NotLearningPostfics))
                    _dictionaryName = _dictionaryName.Replace(Resource.NotLearningPostfics, "");               
                OnPropertyChanged(nameof(DictionaryName)); } }
        private ObservableCollection<Words> _wordsList;
        public ObservableCollection<Words> WordsList { get => _wordsList; set { _wordsList = value; OnPropertyChanged(nameof(WordsList)); } }
        private Words _selectedItem;

        public Words SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; 
                OnPropertyChanged(nameof(SelectedItem));
                SetUnVisibleFloatingMenu();
                if (_selectedItem != null) 
                    ShowActions(_selectedItem);
            }
        }
        private bool _isVisibleListEmpty;
        public bool IsVisibleListEmpty
        {
            get => _isVisibleListEmpty;
            set { _isVisibleListEmpty = value; OnPropertyChanged(nameof(IsVisibleListEmpty)); }
        }



        private async void ShowActions(Words selectedItem)
        {
            try
            {
                SelectedItem = null;
                string remove = Resource.ModalRemoveAct;
                string change = Resource.ModalActChange;               
                var result = await DialogService.ShowActionSheetAsync("", "", Resource.ModalActCancel, buttons: new string[] { change, remove });
                if (result.Equals(change, StringComparison.OrdinalIgnoreCase))
                {
                    await NavigationService.NavigateToAsync<CreateWordViewModel>(selectedItem);
                }
                if (result.Equals(remove, StringComparison.OrdinalIgnoreCase))
                {
                    await Remove(selectedItem);
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }

        public ICommand AddWordCommand { get; set; }
        public ICommand RepeatingWordsCommand { get; set; }
        
        private async Task Remove(Words selectedItem)
        {
            if ( await Task.Run(() => _unitOfWork.WordsRepository.Delete(selectedItem)) )
            {
                _unitOfWork.Save();
                WordsList.Remove(selectedItem);
                SetIsListEmptyLabel();
                OnPropertyChanged(nameof(WordsList));
                CountWords--;
            }
        }

        private int _countWords = 0;
        public int CountWords
        {
            get => _countWords;
            set
            {
                _countWords = value;
                DictionaryName = _dictionary.Name + "(" + CountWords + ")"; 
                OnPropertyChanged(nameof(CountWords));
            }
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            if (navigationData is Dictionary dictionary)
            {
                _dictionary = dictionary;
                DictionaryName = dictionary.Name;
                WordsList = await LoadData(dictionary.Id);
                _countWords = WordsList.Count();
                DictionaryName = dictionary.Name+"("+CountWords+")";
                SetIsListEmptyLabel();
            }
            else
                throw new Exception("Error load words list, bad parameter navigationData to WordsListViewModel");
            await base.InitializeAsync(navigationData);
        }

        private void SetIsListEmptyLabel()
        {
            IsVisibleListEmpty = !WordsList.Any();
        }

        private async Task<ObservableCollection<Words>> LoadData(int id)
        {
            try
            {
               var data = await Task.Run(()=> _unitOfWork.WordsRepository.Get().Where(x=>x.IdDictionary == id ).OrderBy(x => x.RusWord).AsEnumerable());                            
               return new ObservableCollection<Words>(data);                
            }
            catch(Exception e)
            {
                Log.Logger.Error(e);
                return new ObservableCollection<Words>(); 
            }
        }
      
    }
}
