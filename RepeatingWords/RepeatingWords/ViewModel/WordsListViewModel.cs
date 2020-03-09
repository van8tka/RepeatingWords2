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
using RepeatingWords.Services;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class WordsListViewModel : BaseListViewModel
    {
       
        public WordsListViewModel(INavigationService navigationServcie, IDialogService dialogService, IDictionaryStudyService studyService, IImportFile importFile) : base(navigationServcie, dialogService, studyService, importFile)
        {
            AddWordCommand = new Command(async()=> { await NavigationService.NavigateToAsync<CreateWordViewModel>(_dictionary); SetUnVisibleFloatingMenu(); });
            RepeatingWordsCommand = new Command(async()=> { await NavigationService.NavigateToAsync<RepeatingWordsViewModel>(_dictionary); });
            SetUnVisibleFloatingMenu();
        }

        protected override async Task ImportFile(int idLanguage = -1)
        {
            try
            {
                using (var filePiker = await _importFile.PickFile())
                {
                    if (filePiker.DataArray != null)
                        await Task.Run(() => { _importFile.StartImport(filePiker.DataArray, filePiker.FileName, _dictionary.Id); });
                }
                DialogService.HideLoadDialog();
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
            _studyService.RemoveWord(selectedItem);
            WordsList.Remove(selectedItem); 
            SetIsListEmptyLabel();
            OnPropertyChanged(nameof(WordsList));
            CountWords--;
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
                WordsList = LoadData(dictionary.Id);
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

        private ObservableCollection<Words> LoadData(int id)
        {
            var data = _studyService.GetWordsByDictionary(id);                          
            return new ObservableCollection<Words>(data);
        }
      
    }
}
