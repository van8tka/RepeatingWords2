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
            DictionaryList = new ObservableCollection<DictionaryModel>();
            ShowToolsCommand = new Command(async () => { await NavigationService.NavigateToAsync<SettingsViewModel>(); });
            HelperCommand = new Command(async () => { await NavigationService.NavigateToAsync<HelperViewModel>(); });
            LikeCommand = new Command(LikeApplication);
            AddDictionaryCommand = new Command(() => { AddDictionary(); SetUnVisibleFloatingMenu(); });
            AddWordsFromNetCommand = new Command(async () => { await NavigationService.NavigateToAsync<LanguageFrNetViewModel>(); SetUnVisibleFloatingMenu(); });
            AppearingCommand = new Command(async () => await LoadData());
            SetUnVisibleFloatingMenu();
        }

        public ICommand ShowToolsCommand { get; set; }
        public ICommand LikeCommand { get; set; }
        public ICommand HelperCommand { get; set; }
        public ICommand AddDictionaryCommand { get; set; }
        public ICommand AddWordsFromNetCommand { get; set; }
        public ICommand AppearingCommand { get; set; }
        private ObservableCollection<DictionaryModel> _dictionaryList;
        public ObservableCollection<DictionaryModel> DictionaryList { get => _dictionaryList; set { _dictionaryList = value; OnPropertyChanged(nameof(DictionaryList)); } }
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
            //кроме словарей не законченных и словарей недоученных
                var items = await Task.Run(() => _unitOfWork.DictionaryRepository.Get().Where(x => !x.Name.EndsWith(Constants.NAME_DB_FOR_CONTINUE, StringComparison.OrdinalIgnoreCase) && !x.Name.EndsWith(Resource.NotLearningPostfics, StringComparison.OrdinalIgnoreCase)).OrderByDescending(x => x.LastUpdated).AsEnumerable());
                var tempList = new List<DictionaryModel>();
                for (int i = 0; i < items.Count(); i++)
                {
                    var item = items.ElementAt(i);
                    tempList.Add(new DictionaryModel(item));

                }
                DictionaryList = new ObservableCollection<DictionaryModel>(tempList);
        }
        protected override async Task ImportFile()
        {
            try
            {
                int idDictionary = await AddDictionary(isNotImport: false);
                if (idDictionary > 0)
                    if (!await _importFile.PickFile(idDictionary))
                    {
                        await InitializeAsync(null);
                        throw new Exception("Error import words from file");
                    }
                await InitializeAsync(null);
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
                {
                    await NavigationService.NavigateToAsync<RepeatingWordsViewModel>(selectedItem);
                }
                if (result.Equals(studingNotLearning, StringComparison.OrdinalIgnoreCase))
                {
                    await NavigationService.NavigateToAsync<RepeatingWordsViewModel>(unlearningDictionary);
                }
                if (result.Equals(showWords, StringComparison.OrdinalIgnoreCase))
                {
                    await NavigationService.NavigateToAsync<WordsListViewModel>(selectedItem);
                }
                if (result.Equals(removeDictionary, StringComparison.OrdinalIgnoreCase))
                    await RemoveDictionary(selectedItem);
                SelectedItem = null;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }

        private Dictionary GetUnlearningDictionary(Dictionary selected)
        {
           return _unitOfWork.DictionaryRepository.Get().FirstOrDefault(x => x.Name.Equals(selected.Name+Resource.NotLearningPostfics, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<int> AddDictionary(bool isNotImport = true)
        {
            try
            {
                var result = await DialogService.ShowInputTextDialog(Resource.EntryNameDict, Resource.ButtonAddDict, Resource.ButtonCreate, Resource.ModalActCancel);
                if (!string.IsNullOrEmpty(result) || !string.IsNullOrWhiteSpace(result))
                {
                    var dictionary = _unitOfWork.DictionaryRepository.Create(new Dictionary() { Id = 0, Name = result });
                    _unitOfWork.Save();
                    DictionaryList.Add(new DictionaryModel(dictionary));
                    OnPropertyChanged(nameof(DictionaryList));
                    if (isNotImport)
                        await NavigationService.NavigateToAsync<WordsListViewModel>(dictionary);
                    return dictionary.Id;
                }
                return -1;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }

        private async Task RemoveDictionary(Dictionary removeDictionary)
        {
            try
            {
                DialogService.ShowLoadDialog(Resource.Deleting);
                var unlearned = GetUnlearningDictionary(removeDictionary);
                if (unlearned != null)
                   await RemoveDictionary(unlearned);
                var words = await Task.Run(() => _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == removeDictionary.Id).AsEnumerable());
                if (words != null && words.Any())
                    for (int i = 0; i < words.Count(); i++)
                        await Task.Run(() => _unitOfWork.WordsRepository.Delete(words.ElementAt(i)));
                bool success = await Task.Run(() => _unitOfWork.DictionaryRepository.Delete(removeDictionary));
                _unitOfWork.Save();
                if (success)
                {
                    var removed = DictionaryList.Where(x => x.Id == removeDictionary.Id).FirstOrDefault();
                    DictionaryList.Remove(removed);
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


        private async void LikeApplication()
        {
            bool action = await DialogService.ShowConfirmAsync(Resource.MessagePleaseReview, "", Resource.ButtonSendReview, Resource.ModalActCancel);
                if (action)
                {
                    switch (Device.RuntimePlatform)
                    {
                        case Device.Android:
                        {
                            Device.OpenUri(new Uri("https://play.google.com/store/apps/details?id=cardsofwords.cardsofwords"));
                            break;
                        }
                        case Device.UWP:
                        {
                            Device.OpenUri(new Uri("https://www.microsoft.com/store/apps/9n55bwkgshnf"));
                            break;
                        }
                        case Device.iOS:
                        {
                           // Device.OpenUri(new Uri("https://play.google.com/store/apps/details?id=cardsofwords.cardsofwords"));
                            break;
                        }
                    }
                }
        }
    }
}
