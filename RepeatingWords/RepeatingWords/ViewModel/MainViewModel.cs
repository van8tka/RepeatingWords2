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
using System.Threading;

namespace RepeatingWords.ViewModel
{

    /// <summary>
    /// the first page of application
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(INavigationService navService, IDialogService dialogService, IStudyService studyService, IImportFile importFile) : base(navService, dialogService)
        {
            _studyService = studyService;
            _importFile = importFile;
            DictionaryList = new ObservableCollection<LanguageModel>();
            ShowToolsCommand = new Command(async () => { await NavigationService.NavigateToAsync<SettingsViewModel>(); });
            HelperCommand = new Command(async () => { await NavigationService.NavigateToAsync<HelperViewModel>(); });
            LikeCommand = new Command(async () => { await LikeApplication.Like(DialogService); });
            AddLanguageCommand = new Command(async () => { await AddLanguage(); });
            AddWordsFromNetCommand = new Command(async () =>
            {
                await NavigationService.NavigateToAsync<LanguageFrNetViewModel>();
            });
            AddWordCommand = new Command<DictionaryModel>((dictionary) => { NavigationService.NavigateToAsync<CreateWordViewModel>(dictionary); });
            AppearingCommand = new Command(Appearing);
            ShowWordsCommand = new Command<DictionaryModel>(async(dictionary) =>await NavigationService.NavigateToAsync<WordsListViewModel>(dictionary));
            RemoveDictionaryCommand = new Command<DictionaryModel>(async (dictionary) =>await RemoveDictionary(dictionary));
            ImportFromFileCommand = new Command<LanguageModel>((language)=>ImportFile(language.Id));
            RemoveLanguageCommand = new Command<LanguageModel>((language) =>RemoveLanguage(language.Id));
            AddDictionaryCommand = new Command<LanguageModel>((language) =>AddDictionary(language.Id));
            ContextActionsMenuCommand = new Command<LanguageModel>((language) => ContextActionsMenu(language));

        }

        private readonly IStudyService _studyService;
        private readonly IImportFile _importFile;

        public void Appearing()
        {
            LoadData();
        }
        public ICommand ImportFromFileCommand { get; set; }
        public ICommand RemoveLanguageCommand { get; set; }
        public ICommand AddDictionaryCommand { get; set; }
        public ICommand AddWordCommand { get; set; }
        public ICommand ShowToolsCommand { get; set; }
        public ICommand LikeCommand { get; set; }
        public ICommand HelperCommand { get; set; }
        public ICommand AddLanguageCommand { get; set; }
        public ICommand AddWordsFromNetCommand { get; set; }
        public ICommand AppearingCommand { get; set; }
        public ICommand ShowWordsCommand { get; set; }
        public ICommand RemoveDictionaryCommand { get; set; }
        public ICommand ContextActionsMenuCommand { get; set; }



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
                if(value!=null)
                    StudyDictionary(_selectedItem);
            }
        }

       
        public override Task InitializeAsync(object navigationData)
        {
            try
            {
                IsBusy = true;
                if ( FirstStartService.IsFirstStart())
                    ShowDialogSetup();
                LoadData();
            }
            catch (Exception er)
            {
                Log.Logger.Error(er);
            }
            return base.InitializeAsync(navigationData);
        }

        private async void ShowDialogSetup()
        {
            string btnSetup = Resource.ButtonSetup;
            string btnLate = Resource.ButtonLater;
            string titleNeedSetup = Resource.LabelNeedSetup;
            var result = await DialogService.ShowConfirmAsync(titleNeedSetup,
                "", btnSetup, btnLate);
            if (result)
                await NavigationService.NavigateToAsync<SettingsViewModel>();
        }

        private void LoadData()
        {
            DictionaryList = _studyService.DictionaryList;
        }


       private async void ContextActionsMenu(LanguageModel language)
       {
            try
            {
                string add = Resource.ButtonCreate;
                string remove = Resource.ButtonRemove;
                string import = Resource.ButtonImport;
                var action = await DialogService.ShowActionSheetAsync("", Resource.ModalActCancel, "", CancellationToken.None , add, remove, import);
                if (action.Equals(add))
                    AddDictionaryCommand.Execute(language);
                else if (action.Equals(import))
                    ImportFromFileCommand.Execute(language);
                else if (action.Equals(remove))
                    RemoveLanguageCommand.Execute(language);

            }
            catch(Exception er)
            {
                Log.Logger.Error(er);
            }
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
                        if (filePiker != null && filePiker.DataArray != null)
                            await Task.Run(() =>
                            {
                                _importFile.StartImport(filePiker.DataArray, filePiker.FileName, idDictionary);
                            });
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

        private async void StudyDictionary(DictionaryModel dictionary)
        {
            try
            {
                dictionary.IsStudyUnlearnedWords = false;
                await NavigationService.NavigateToAsync<RepeatingWordsViewModel>(dictionary);
                SelectedItem = null;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }
        
 


        public async Task<int> AddDictionary(int idLanguage)
        {
            var dictionaryName = await DialogService.ShowInputTextDialog(Resource.EntryNameDict, Resource.ButtonAddDict,
                Resource.ButtonCreate, Resource.ModalActCancel);
            if (!string.IsNullOrEmpty(dictionaryName))
            {
                int id = _studyService.AddDictionary(dictionaryName, idLanguage);
                var isExp = DictionaryList.FirstOrDefault(x => x.Id == idLanguage).Expanded;
                if (!isExp)
                    DictionaryList.FirstOrDefault(x => x.Id == idLanguage).Expanded = true;
                return id;
            }
            return -1;
        }



        private async Task<int> AddLanguage()
        {
            var nameLang = await DialogService.ShowInputTextDialog(Resource.EntryNameLang, Resource.BtnAddLang,
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
                await Task.Run(()=> _studyService.RemoveDictionaryFromLanguage(removeDictionary.Id));
                _studyService.CommitTransaction();
                DictionaryList.FirstOrDefault(x => x.Id == removeDictionary.IdLanguage)
                    ?.RemoveDictionary(removeDictionary.Id);
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
            Log.Logger.Info($"remove language with id = {idlanguage}");
            DialogService.ShowLoadDialog(Resource.Deleting);
            _studyService.BeginTransaction();
            if (await Task.Run(()=> _studyService.RemoveLanguage(idlanguage)))
                _studyService.CommitTransaction();
            else
                _studyService.RollBackTransaction();
            OnPropertyChanged(nameof(DictionaryList));
            DialogService.HideLoadDialog();
        }

    }
}
