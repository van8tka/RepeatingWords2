using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using Xamarin.Forms;

namespace RepeatingWords.ViewModel
{
    public class DictionariesListViewModel : BaseListViewModel
    {
        public DictionariesListViewModel(INavigationService navigationServcie, IDialogService dialogService, IUnitOfWork unitOfWork, IImportFile importFile) : base(navigationServcie, dialogService,unitOfWork, importFile)
        {
            DictionaryList = new ObservableCollection<Dictionary>();
            AddDictionaryCommand = new Command(()=>{ AddDictionary();SetUnVisibleFloatingMenu(); });
            AddWordsFromNetCommand = new Command(async()=> { await NavigationService.NavigateToAsync<LanguageFrNetViewModel>(); SetUnVisibleFloatingMenu();  });
            SetUnVisibleFloatingMenu();
        }

        public ICommand AddDictionaryCommand { get; set; }
        public ICommand AddWordsFromNetCommand { get; set; }
        private ObservableCollection<Dictionary> _dictionaryList;
        public ObservableCollection<Dictionary> DictionaryList { get => _dictionaryList; set { _dictionaryList = value; OnPropertyChanged(nameof(DictionaryList)); } }
        private Dictionary _selectedItem;
        public Dictionary SelectedItem { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(nameof(SelectedItem)); if (_selectedItem != null) ShowActions(_selectedItem); }  }

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

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            DictionaryList = await LoadData();
            await base.InitializeAsync(navigationData);
        }

       private async Task<int> AddDictionary(bool isNotImport = true )
        {
            try
            {
                var result = await DialogService.ShowInputTextDialog(Resource.EntryNameDict, Resource.ButtonAddDict, Resource.ButtonCreate, Resource.ModalActCancel);
                if (!string.IsNullOrEmpty(result) || !string.IsNullOrWhiteSpace(result))
                {
                   var dictionary = _unitOfWork.DictionaryRepository.Create(new Dictionary() { Id = 0, Name = result });
                    _unitOfWork.Save();
                    DictionaryList.Add(dictionary);
                    OnPropertyChanged(nameof(DictionaryList));
                    if ( isNotImport )
                        await NavigationService.NavigateToAsync<WordsListViewModel>(dictionary);
                    return dictionary.Id;
                }
                return -1;
            }
            catch(Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }                        
        }

        private async Task<ObservableCollection<Dictionary>> LoadData()
        {
            try
            {             
                //кроме словарей не законченных и словарей недоученных
                var items = await Task.Run(()=> _unitOfWork.DictionaryRepository.Get().Where(x => !x.Name.EndsWith(Constants.NAME_DB_FOR_CONTINUE, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.Name).AsEnumerable());
                return new ObservableCollection<Dictionary>(items);             
            }
            catch(Exception e)
            {
                Log.Logger.Error(e);
                return new ObservableCollection<Dictionary>();
            }
        }


        private async void ShowActions(Dictionary selectedItem)
        {
            try
            {
                SelectedItem = null;
                string removeDictionary = Resource.ButtonRemove;
                string showWords = Resource.ButtonShowWords;
                string studing = Resource.ButtonRepeatWords;
                var result = await DialogService.ShowActionSheetAsync("","", Resource.ModalActCancel ,buttons:new string[] { showWords, studing, removeDictionary });
                if(result.Equals(studing, StringComparison.OrdinalIgnoreCase))
                {
                    await NavigationService.NavigateToAsync<RepeatingWordsViewModel>(selectedItem);
                }
                if (result.Equals(showWords, StringComparison.OrdinalIgnoreCase))
                {
                    await NavigationService.NavigateToAsync<WordsListViewModel>(selectedItem);
                }
                if (result.Equals(removeDictionary, StringComparison.OrdinalIgnoreCase))
                    await RemoveDictionary(selectedItem);
            }
            catch(Exception e)
            {
                Log.Logger.Error(e);
            }
        }

        private async Task RemoveDictionary(Dictionary removeDictionary)
        {
            try
            {
                DialogService.ShowLoadDialog(Resource.Deleting);
                var words = await Task.Run(() => _unitOfWork.WordsRepository.Get().Where(x=>x.IdDictionary == removeDictionary.Id).AsEnumerable());
                if(words!=null && words.Any())
                    for(int i=0;i<words.Count();i++)
                        await Task.Run(() => _unitOfWork.WordsRepository.Delete(words.ElementAt(i)));
                var lastRepeat = _unitOfWork.LastActionRepository.Get().LastOrDefault();
                if(lastRepeat!=null && lastRepeat.IdDictionary == removeDictionary.Id)               
                    await Task.Run(() => _unitOfWork.LastActionRepository.Delete(lastRepeat));
                bool success = await Task.Run(()=> _unitOfWork.DictionaryRepository.Delete(removeDictionary));
                _unitOfWork.Save();
                if (success)
                    DictionaryList.Remove(removeDictionary);
                OnPropertyChanged(nameof(DictionaryList));
                DialogService.HideLoadDialog();
            }
            catch(Exception e)
            {
                DialogService.HideLoadDialog();
                Log.Logger.Error(e);
            }
        }       
    }
}
