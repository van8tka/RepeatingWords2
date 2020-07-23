using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepeatingWords.Model;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.Services;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class WordsListViewModel:ViewModelBase
    {
       //ctor
        public WordsListViewModel(INavigationService navigationServcie, IDialogService dialogService, IStudyService studyService, IImportFile importFile) : base(navigationServcie, dialogService)
        {
            _studyService = studyService ?? throw new ArgumentNullException(nameof(studyService));
            _importFile = importFile ?? throw new ArgumentNullException(nameof(importFile));
            AddWordCommand = new Command(()=> { NavigationService.NavigateToAsync<CreateWordViewModel>(_dictionary); });
            RepeatingWordsCommand = new Command(()=> { NavigationService.NavigateToAsync<RepeatingWordsViewModel>(_dictionary); });
            ImportWordsCommand = new Command( async () => { DialogService.ShowLoadDialog(); await ImportFile(); });
            ChangeWordCommand = new Command<WordsModel>((selectedItem)=> NavigationService.NavigateToAsync<CreateWordViewModel>(selectedItem));
            RemoveWordCommand = new Command<WordsModel>((s)=> Remove(s));
        }
        public ICommand AddWordCommand { get; set; }
        public ICommand RepeatingWordsCommand { get; set; }
        public ICommand ImportWordsCommand { get; set; }
        public ICommand ChangeWordCommand { get; set; }
        public ICommand RemoveWordCommand { get; set; }


        private readonly IStudyService _studyService;
        private readonly IImportFile _importFile;

        private DictionaryModel _dictionary;
        private string _dictionaryName;
        public string DictionaryName { get => _dictionaryName;
            set {
                _dictionaryName = value;
                OnPropertyChanged(nameof(DictionaryName)); } }
        private ObservableCollection<WordsModel> _wordsList;
        public ObservableCollection<WordsModel> WordsList { get => _wordsList; set { _wordsList = value; OnPropertyChanged(nameof(WordsList)); } }
        private WordsModel _selectedItem;
        public WordsModel SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; 
                OnPropertyChanged(nameof(SelectedItem));
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

        private async void ShowActions(WordsModel selectedItem)
        {
            try
            {
                SelectedItem = null;
                string remove = Resource.ModalRemoveAct;
                string change = Resource.ModalActChange;               
                var result = await DialogService.ShowActionSheetAsync("", "", Resource.ModalActCancel, buttons: new string[] { change, remove });
                if (result.Equals(change, StringComparison.OrdinalIgnoreCase))
                    ChangeWordCommand.Execute(selectedItem);
                if (result.Equals(remove, StringComparison.OrdinalIgnoreCase))
                    RemoveWordCommand.Execute(selectedItem);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }

        private Task Remove(WordsModel selectedItem)
        {
            WordsList.Remove(selectedItem);
            var task = Task.Run(()=>_studyService.RemoveWord(selectedItem));
            SetIsListEmptyLabel();
            OnPropertyChanged(nameof(WordsList));
            CountWords--;
            return task;
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

        public override Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            _dictionary = navigationData as DictionaryModel;
            CountWords = _dictionary?.CountWords??0;
            WordsList = _dictionary?.WordsCollection??new ObservableCollectionListSource<WordsModel>();
            SetIsListEmptyLabel();
            return base.InitializeAsync(navigationData);
        }

        protected async Task ImportFile()
        {
            try
            {
                using (var filePiker = await _importFile.PickFile())
                {
                    bool success = false;
                    if (filePiker != null && filePiker.DataArray != null)
                        success = await _importFile.StartImport(filePiker.DataArray, filePiker.FileName, _dictionary.Id);
                    if(!success)
                        DialogService.ShowToast(Resource.ErrorImport);
                }
                DialogService.HideLoadDialog();
                await InitializeAsync(_dictionary);
            }
            catch (Exception er)
            {
                DialogService.HideLoadDialog();
                DialogService.ShowToast(Resource.ErrorImport);
                Log.Logger.Error(er);
            }
        }

        private void SetIsListEmptyLabel()
        {
            IsVisibleListEmpty = !WordsList.Any();
        }

    }
}
