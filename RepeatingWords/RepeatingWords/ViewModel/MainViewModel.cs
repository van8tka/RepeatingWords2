using RepeatingWords.DataService.Interfaces;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using RepeatingWords.DataService.Model;
using RepeatingWords.Heleprs;
using RepeatingWords.Helpers;
using RepeatingWords.Interfaces;
using RepeatingWords.Model;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{

    /// <summary>
    /// the first page of application
    /// </summary>
    public class MainViewModel : BaseListViewModel
    {
        public MainViewModel( INavigationService navService, IDialogService dialogService, IUnitOfWork unitOfWork, IImportFile importFile) : base(navService, dialogService, unitOfWork, importFile)
        {
            DictionaryList = new ObservableCollection<LanguageModel>();
            ShowToolsCommand = new Command(async () => { await NavigationService.NavigateToAsync<SettingsViewModel>(); });
            HelperCommand = new Command(async () => { await NavigationService.NavigateToAsync<HelperViewModel>(); });
            LikeCommand = new Command(async ()=> { await LikeApplication.Like(DialogService); });
            AddLanguageCommand = new Command(() => { AddLanguage(); SetUnVisibleFloatingMenu(); });
            AddWordsFromNetCommand = new Command(async () => { await NavigationService.NavigateToAsync<LanguageFrNetViewModel>(); SetUnVisibleFloatingMenu(); });
            AppearingCommand = new Command(async () => await LoadData());
            ContextMenuLanguageCommand = new Command<int>(async(id)=>await ContextMenuLanguage(id));
            SetUnVisibleFloatingMenu();
        }

        public ICommand ShowToolsCommand { get; set; }
        public ICommand LikeCommand { get; set; }
        public ICommand HelperCommand { get; set; }
        public ICommand AddLanguageCommand { get; set; }
        public ICommand ContextMenuLanguageCommand { get; set; }
        public ICommand AddWordsFromNetCommand { get; set; }
        public ICommand AppearingCommand { get; set; }

        private ObservableCollection<LanguageModel> _dictionaryList;
        public ObservableCollection<LanguageModel> DictionaryList { get => _dictionaryList; set { _dictionaryList = value; OnPropertyChanged(nameof(DictionaryList)); } }
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
            await base.InitializeAsync(navigationData);
        }
        private async Task LoadData()
        {
            DictionaryList.Clear();
            var langs = _unitOfWork.LanguageRepository.Get();
            foreach (var lang in langs)
            {
                var items = await Task.Run(() => _unitOfWork.DictionaryRepository.Get().Where(x =>x.IdLanguage==lang.Id && !x.Name.EndsWith(Constants.NAME_DB_FOR_CONTINUE, StringComparison.OrdinalIgnoreCase) && !x.Name.EndsWith(Resource.NotLearningPostfics, StringComparison.OrdinalIgnoreCase)).OrderByDescending(x => x.LastUpdated).AsEnumerable());
                var langModel = new LanguageModel(DialogService,_unitOfWork,lang, items, false);
                DictionaryList.Add(langModel);
            }
        }

        protected override async Task ImportFile( )
        {
            throw new NotImplementedException();
        }
        protected async Task ImportFromFile(int idLanguage)
        {
            try
            {
                var lang = DictionaryList.Where(x => x.Id == idLanguage).FirstOrDefault();
                var isAddDictionary = await DictionaryList.Where(x => x.Id == idLanguage).FirstOrDefault().AddDictionaryToLanguage();
                if (isAddDictionary)
                {
                    int idDictionary = _unitOfWork.DictionaryRepository.Get().Where(x => x.IdLanguage == idLanguage).OrderByDescending(x => x.LastUpdated).FirstOrDefault().Id;
                    if (idDictionary > 0)
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
                var selectedItem = _unitOfWork.DictionaryRepository.Get(idDictionary);
                string removeDictionary = Resource.ButtonRemove;
                string showWords = Resource.ButtonShowWords;
                string studing = Resource.ButtonRepeatWords;
                string studingNotLearning = Resource.ButtonStudyNotLearning;
                var unlearningDictionary = GetUnlearningDictionary(selectedItem);
                string[] actionButtons;
                if (unlearningDictionary != null)
                    actionButtons = new string[] {studing, studingNotLearning, showWords, removeDictionary};
                else
                    actionButtons = new string[] { studing, showWords, removeDictionary };
                var result = await DialogService.ShowActionSheetAsync("", "", Resource.ModalActCancel, buttons: actionButtons);
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

                string[] actionButtons = new string[] { removeLanguage, addDictionary, addFromFile };
                var result = await DialogService.ShowActionSheetAsync("", "", Resource.ModalActCancel, buttons: actionButtons);
                if (result.Equals(removeLanguage, StringComparison.OrdinalIgnoreCase))
                    await RemoveLanguage(idLanguage);
                else if (result.Equals(addDictionary, StringComparison.OrdinalIgnoreCase))
                    await DictionaryList.Where(x => x.Id == idLanguage).FirstOrDefault().AddDictionaryToLanguage();
                else if (result.Equals(addFromFile, StringComparison.OrdinalIgnoreCase))
                    await ImportFromFile(idLanguage);
                return true;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                return false;
            }
        }


        private Dictionary GetUnlearningDictionary(Dictionary selected)
        {
           return _unitOfWork.DictionaryRepository.Get().FirstOrDefault(x => x.Name.Equals(selected.Name+Resource.NotLearningPostfics, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<int> AddLanguage(bool isNotImport = true)
        {
            try
            {
                var result = await DialogService.ShowInputTextDialog(Resource.EntryNameDict, Resource.ButtonAddDict, Resource.ButtonCreate, Resource.ModalActCancel);
                if (!string.IsNullOrEmpty(result) || !string.IsNullOrWhiteSpace(result))
                {
                    var language = _unitOfWork.LanguageRepository.Create(new Language() { Id = 0, NameLanguage = result, PercentOfLearned = 0 });
                    _unitOfWork.Save();
                    DictionaryList.Add(new LanguageModel(DialogService, _unitOfWork, language));
                    OnPropertyChanged(nameof(DictionaryList));
                    return language.Id;
                }
                return -1;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                return -1;
            }
        }

        private async Task RemoveDictionary(Dictionary removeDictionary)
        {
            try
            {
                DialogService.ShowLoadDialog(Resource.Deleting);
                await DictionaryList.Where(x => x.Id == removeDictionary.IdLanguage).FirstOrDefault().RemoveDictionaryFromLanguage(removeDictionary);
                OnPropertyChanged(nameof(DictionaryList));
                DialogService.HideLoadDialog();
            }
            catch (Exception e)
            {
                DialogService.HideLoadDialog();
                Log.Logger.Error(e);
            }
        }

        private async Task RemoveLanguage(int idlanguage)
        {
            try
            {
                    DialogService.ShowLoadDialog(Resource.Deleting);
                    var removedLanguage = DictionaryList.Where(x => x.Id == idlanguage).FirstOrDefault();
                    if (removedLanguage != null)
                    {
                        var dictionaries = _unitOfWork.DictionaryRepository.Get().Where(x => x.IdLanguage == idlanguage).AsEnumerable();
                        foreach (var item in dictionaries)
                            await removedLanguage.RemoveDictionaryFromLanguage(item);
                        var language = _unitOfWork.LanguageRepository.Get(removedLanguage.Id);
                        _unitOfWork.LanguageRepository.Delete(language);
                        _unitOfWork.Save();
                        DictionaryList.Remove(removedLanguage);
                    }
                    OnPropertyChanged(nameof(DictionaryList));
                    DialogService.HideLoadDialog();
            }
            catch (Exception e)
            {
                DialogService.HideLoadDialog();
                Log.Logger.Error(e);
            }
        }

    }
}
