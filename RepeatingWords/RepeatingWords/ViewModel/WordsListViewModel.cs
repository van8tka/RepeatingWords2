using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class WordsListViewModel : ViewModelBase
    {
       
        public WordsListViewModel(INavigationService navigationServcie, IDialogService dialogService, IUnitOfWork unitOfWork) : base(navigationServcie, dialogService)
        {
            _unitOfWork = unitOfWork;
            AddWordCommand = new Command(async()=> { await NavigationService.NavigateToAsync<CreateWordViewModel>(_dictionary); });
            ImportWordsCommand = new Command(async () => { await NavigationService.NavigateToAsync<ChooseFileViewModel>(_dictionary); });
            RepeatingWordsCommand = new Command(async()=> { await NavigationService.NavigateToAsync<RepeatingWordsViewModel>(_dictionary); });
        }
        private readonly IUnitOfWork _unitOfWork;
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
        public Words SelectedItem { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(nameof(SelectedItem));if (_selectedItem != null) ShowActions(_selectedItem); } }

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
        public ICommand ImportWordsCommand { get; set; }

        private async Task Remove(Words selectedItem)
        {
            if ( await Task.Run(() => _unitOfWork.WordsRepository.Delete(selectedItem)) )
            {
                _unitOfWork.Save();
                WordsList.Remove(selectedItem);
                OnPropertyChanged(nameof(WordsList));
            }
        }
      
    
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            if (navigationData is Dictionary dictionary)
            {
                _dictionary = dictionary;
                DictionaryName = dictionary.Name;
                await LoadData(dictionary.Id);
            }
            else
                throw new Exception("Error load words list, bad parameter navigationData to WordsListViewModel");
               
            await base.InitializeAsync(navigationData);
        }

        private async Task LoadData(int id)
        {
            try
            {
               var data = await Task.Run(()=> _unitOfWork.WordsRepository.Get().Where(x=>x.IdDictionary == id ).OrderBy(x => x.RusWord).AsEnumerable());              
               var list = new ObservableCollection<Words>();
               for (int i = 0; i < data.Count(); i++)
                   list.Add(data.ElementAt(i));
                WordsList = list;                
            }
            catch(Exception e)
            {
                Log.Logger.Error(e);
            }
        }
      
    }
}
