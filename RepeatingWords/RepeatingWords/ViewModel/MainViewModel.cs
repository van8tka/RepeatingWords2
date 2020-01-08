using RepeatingWords.DataService.Interfaces;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using RepeatingWords.DataService.Model;
using RepeatingWords.Heleprs;
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
    public class MainViewModel : BaseListViewModel
    {
        public MainViewModel(INavigationService navService, IDialogService dialogService,
            IDictionaryStudyService studyService, IImportFile importFile) : base(navService, dialogService,
            studyService, importFile)
        {
            DictionaryList = new ObservableCollection<LanguageModel>();
            ShowToolsCommand =
                new Command(async () => { await NavigationService.NavigateToAsync<SettingsViewModel>(); });
            HelperCommand = new Command(async () => { await NavigationService.NavigateToAsync<HelperViewModel>(); });
            LikeCommand = new Command(async () => { await LikeApplication.Like(DialogService); });
            AddLanguageCommand = new Command(async() =>
            {
              await AddLanguage();
                SetUnVisibleFloatingMenu();
            });
            AddWordsFromNetCommand = new Command(async () =>
            {
                await NavigationService.NavigateToAsync<LanguageFrNetViewModel>();
                SetUnVisibleFloatingMenu();
            });
            //  AppearingCommand = new Command(async () => await LoadData());
            ContextMenuLanguageCommand = new Command<int>(async (id) => await ContextMenuLanguage(id));
            SetUnVisibleFloatingMenu();
        }

        public ICommand ShowToolsCommand { get; set; }
        public ICommand LikeCommand { get; set; }
        public ICommand HelperCommand { get; set; }
        public ICommand AddLanguageCommand { get; set; }
        public ICommand ContextMenuLanguageCommand { get; set; }
        public ICommand AddWordsFromNetCommand { get; set; }
       

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
                SetUnVisibleFloatingMenu();
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


        protected override async Task ImportFile(int idLanguage)
        {
            try
            {
                int idDictionary = await AddDictionary(idLanguage);
                if (idDictionary > 0)
                {
                    if (!await _importFile.PickFile(idDictionary))
                    {
                        await InitializeAsync(null);
                        throw new Exception("Error import words from file");
                    }
                    await InitializeAsync(null);
                }
            }
            catch (Exception er)
            {
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
                var unlearningDictionary = _studyService.GetUnlearningDictionary(selectedItem.Name);
                string[] actionButtons;
                if (unlearningDictionary != null)
                    actionButtons = new string[] {studing, studingNotLearning, showWords, removeDictionary};
                else
                    actionButtons = new string[] {studing, showWords, removeDictionary};
                var result =
                    await DialogService.ShowActionSheetAsync("", "", Resource.ModalActCancel, buttons: actionButtons);
                if (result.Equals(studing, StringComparison.OrdinalIgnoreCase))
                    await NavigationService.NavigateToAsync<RepeatingWordsViewModel>(selectedItem);
                else if (result.Equals(studingNotLearning, StringComparison.OrdinalIgnoreCase))
                    await NavigationService.NavigateToAsync<RepeatingWordsViewModel>(unlearningDictionary);
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
                    await ImportFile(idLanguage);
                return true;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                return false;
            }
        }


        public async Task<int> AddDictionary(int idLanguage)
        {
          
            var dictionaryName = await DialogService.ShowInputTextDialog(Resource.EntryNameDict, Resource.ButtonAddDict, Resource.ButtonCreate, Resource.ModalActCancel); 
            return _studyService.AddDictionary(dictionaryName, idLanguage);
        }



        private async Task<int> AddLanguage()
        {
            var nameLang =  await DialogService.ShowInputTextDialog(Resource.EntryNameLang, Resource.BtnAddLang,
                Resource.ButtonCreate, Resource.ModalActCancel);
            int idLang = _studyService.AddLanguage(nameLang);
            OnPropertyChanged(nameof(DictionaryList));
            return idLang;
        }

        private async Task RemoveDictionary(Dictionary removeDictionary)
        {
            DialogService.ShowLoadDialog(Resource.Deleting);
            await _studyService.RemoveDictionaryFromLanguage(removeDictionary,
                DictionaryList.FirstOrDefault(x => x.Id == removeDictionary.IdLanguage));
            OnPropertyChanged(nameof(DictionaryList));
            DialogService.HideLoadDialog();
        }

        private async Task RemoveLanguage(int idlanguage)
        {
            DialogService.ShowLoadDialog(Resource.Deleting);
            await _studyService.RemoveLanguage(idlanguage);
            OnPropertyChanged(nameof(DictionaryList));
            DialogService.HideLoadDialog();
        }

    }
}
