using RepeatingWords.DataService.Interfaces;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers;
using RepeatingWords.Interfaces;
using RepeatingWords.Model;
using RepeatingWords.Services;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{

    /// <summary>
    /// the first page of application
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(INavigationService navService, IDialogService dialogService,
            IStudyService studyService, IImportFile importFile) : base(navService, dialogService)
        {
            _studyService = studyService;
            _importFile = importFile;
            DictionaryList = new ObservableCollection<LanguageModel>();
            ShowToolsCommand = new Command(async () => { await NavigationService.NavigateToAsync<SettingsViewModel>(); });
            HelperCommand = new Command(async () => { await NavigationService.NavigateToAsync<HelperViewModel>(); });
            LikeCommand = new Command(async () => { await LikeApplication.Like(DialogService); });
            AddLanguageCommand = new Command(async() =>
            {
              await AddLanguage();
               
            });
            AddWordsFromNetCommand = new Command(async () =>
            {
                await NavigationService.NavigateToAsync<LanguageFrNetViewModel>();
            });
            AppearingCommand = new Command(Appearing);
            ContextMenuLanguageCommand = new Command<int>(async (id) => await ContextMenuLanguage(id));
            
        }

        private readonly IStudyService _studyService;
        private readonly IImportFile _importFile;

        public void Appearing()
        {
            LoadData();
        }

        public ICommand ShowToolsCommand { get; set; }
        public ICommand LikeCommand { get; set; }
        public ICommand HelperCommand { get; set; }
        public ICommand AddLanguageCommand { get; set; }
        public ICommand ContextMenuLanguageCommand { get; set; }
        public ICommand AddWordsFromNetCommand { get; set; }
        public ICommand AppearingCommand { get; set; }
        
        private ObservableCollection<LanguageModel> _dictionaryList;

        public ObservableCollection<LanguageModel> DictionaryList
        {
            get => _dictionaryList;
            set
            {
                _dictionaryList = value;
                OnPropertyChanged(nameof(DictionaryList));
            }
        }

        private DictionaryModel _selectedItem;

        public DictionaryModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                if (_selectedItem != null)
                    ContextMenuDictionary(_selectedItem.Id);
            }
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true; 
            LoadData();
            await base.InitializeAsync(navigationData);
        }

        private void LoadData()
        {
            DictionaryList = _studyService.DictionaryList;
        }


        protected async Task ImportFile(int idLanguage)
        {
            try
            {
                int idDictionary = await AddDictionary(idLanguage);
                if (idDictionary > 0)
                {
                    using (var filePiker = await _importFile.PickFile())
                    {
                        if (filePiker!=null && filePiker.DataArray != null)
                            await Task.Run(() => { _importFile.StartImport(filePiker.DataArray, filePiker.FileName, idDictionary); });
                    }
                    await InitializeAsync(null);
                }
                DialogService.HideLoadDialog();
            }
            catch (Exception er)
            {
                DialogService.HideLoadDialog();
                DialogService.ShowToast(Resource.ErrorImport);
                Log.Logger.Error(er);
            }
        }

        private async void ContextMenuDictionary(int idDictionary)
        {
            try
            {
                var selectedItem = _studyService.GetDictionary(idDictionary);
                string removeDictionary = Resource.ButtonRemove;
                string showWords = Resource.ButtonShowWords;
                string studing = Resource.ButtonRepeatWords;
                string studingNotLearning = Resource.ButtonStudyNotLearning;
                string[] actionButtons;
                if (selectedItem.CountLearned > 0 && selectedItem.CountLearned < selectedItem.CountWords)
                    actionButtons = new string[] { studing, studingNotLearning, showWords, removeDictionary };
                else
                    actionButtons = new string[] {studing, showWords, removeDictionary};
                var result =
                    await DialogService.ShowActionSheetAsync("", "", Resource.ModalActCancel, buttons: actionButtons);
                if (result.Equals(studing, StringComparison.OrdinalIgnoreCase))
                {
                    selectedItem.IsStudyUnlearnedWords = false;
                    await NavigationService.NavigateToAsync<RepeatingWordsViewModel>(selectedItem);
                }
                else if (result.Equals(studingNotLearning, StringComparison.OrdinalIgnoreCase))
                {
                    selectedItem.IsStudyUnlearnedWords = true;
                    await NavigationService.NavigateToAsync<RepeatingWordsViewModel>(selectedItem);
                }
                else if (result.Equals(showWords, StringComparison.OrdinalIgnoreCase))
                    await NavigationService.NavigateToAsync<WordsListViewModel>(selectedItem);
                else if (result.Equals(removeDictionary, StringComparison.OrdinalIgnoreCase))
                    await RemoveDictionary(selectedItem);
                SelectedItem = null;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }

        private async Task<bool> ContextMenuLanguage(int idLanguage)
        {
            try
            {
                string removeLanguage = Resource.ButtonRemoveLanguage;
                string addDictionary = Resource.ButtonAddDict;
                string addFromFile = Resource.ButtonImport;

                string[] actionButtons = new string[] {removeLanguage, addDictionary, addFromFile};
                var result =
                    await DialogService.ShowActionSheetAsync("", "", Resource.ModalActCancel, buttons: actionButtons);
                if (result.Equals(removeLanguage, StringComparison.OrdinalIgnoreCase))
                    await RemoveLanguage(idLanguage);
                else if (result.Equals(addDictionary, StringComparison.OrdinalIgnoreCase))
                    await AddDictionary(idLanguage);
                else if (result.Equals(addFromFile, StringComparison.OrdinalIgnoreCase))
                {
                    DialogService.ShowLoadDialog();
                    await ImportFile(idLanguage);
                }
                DialogService.HideLoadDialog();
                return true;
            }
            catch (Exception e)
            {
                DialogService.HideLoadDialog();
                Log.Logger.Error(e);
                return false;
            }
        }


        public async Task<int> AddDictionary(int idLanguage)
        {
            var dictionaryName = await DialogService.ShowInputTextDialog(Resource.EntryNameDict, Resource.ButtonAddDict, Resource.ButtonCreate, Resource.ModalActCancel); 
            if(!string.IsNullOrEmpty(dictionaryName))
                return _studyService.AddDictionary(dictionaryName, idLanguage);
            return -1;
        }



        private async Task<int> AddLanguage()
        {
            var nameLang =  await DialogService.ShowInputTextDialog(Resource.EntryNameLang, Resource.BtnAddLang,
                Resource.ButtonCreate, Resource.ModalActCancel);
            int idLang = _studyService.AddLanguage(nameLang);
            OnPropertyChanged(nameof(DictionaryList));
            return idLang;
        }

        private async Task RemoveDictionary(DictionaryModel removeDictionary)
        {
            try
            {
                Log.Logger.Info("remove dictionary");
                DialogService.ShowLoadDialog(Resource.Deleting);
                OnPropertyChanged(nameof(DictionaryList));
                _studyService.BeginTransaction();
                await _studyService.RemoveDictionaryFromLanguage(removeDictionary.Id);
                _studyService.CommitTransaction();
                DictionaryList.FirstOrDefault(x => x.Id == removeDictionary.IdLanguage)?.RemoveDictionary(removeDictionary.Id);
                DialogService.HideLoadDialog();
            }
            catch (Exception e)
            {
                _studyService.RollBackTransaction();
                DialogService.HideLoadDialog();
                DialogService.ShowToast("Error remove dictionary");
                Log.Logger.Error(e);
            }
        }

        private async Task RemoveLanguage(int idlanguage)
        {
            try
            {
                Log.Logger.Info($"remove language with id = {idlanguage}");
                DialogService.ShowLoadDialog(Resource.Deleting);
                _studyService.BeginTransaction();
                await _studyService.RemoveLanguage(idlanguage);
                _studyService.CommitTransaction();
                OnPropertyChanged(nameof(DictionaryList));
                DialogService.HideLoadDialog();
            }
            catch (Exception e)
            {
                _studyService.RollBackTransaction();
                DialogService.HideLoadDialog();
                DialogService.ShowToast("Error remove language");
                Log.Logger.Error(e);
            }
        }

    }
}
