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
    public class DictionariesListViewModel : ViewModelBase
    {     
        private readonly IUnitOfWork _unitOfWork;
        //string NotLearningWords = Constants.NAME_DB_FOR_CONTINUE + Resource.NotLearningPostfics;

        public DictionariesListViewModel(INavigationService navigationServcie, IDialogService dialogService, IUnitOfWork unitOfWork) : base(navigationServcie, dialogService)
        {
            _unitOfWork = unitOfWork;
            DictionaryList = new ObservableCollection<Dictionary>();
            LoadData();
            AddDictionaryCommand = new Command(AddDictionary);
            AddWordsFromNetCommand = new Command(async()=> { await AddWordsFromNet(); });           
        }

        public override Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            return base.InitializeAsync(navigationData);
        }

        public ICommand AddDictionaryCommand { get; set; }
        public ICommand AddWordsFromNetCommand { get; set; }        

        private ObservableCollection<Dictionary> _dictionaryList;
        public ObservableCollection<Dictionary> DictionaryList { get => _dictionaryList; set { _dictionaryList = value; OnPropertyChanged(nameof(DictionaryList)); } }
        private Dictionary _selectedItem;
        public Dictionary SelectedItem { get => _selectedItem; set { _selectedItem = value; OnPropertyChanged(nameof(SelectedItem)); if (_selectedItem != null) ShowActions(_selectedItem); }  }
      
        private async Task AddWordsFromNet( )
        {                     
            await NavigationService.NavigateToAsync<LanguageFrNetViewModel>();                                  
        }
        private async void AddDictionary( )
        {
            try
            {
                var result = await DialogService.ShowInputTextDialog(Resource.EntryNameDict, Resource.ButtonAddDict, Resource.ButtonCreate, Resource.ModalActCancel);
                if (!string.IsNullOrEmpty(result) || !string.IsNullOrWhiteSpace(result))
                {
                    _unitOfWork.DictionaryRepository.Create(new Dictionary() { Id = 0, Name = result });
                    _unitOfWork.Save();
                    var lastDictionary = _unitOfWork.DictionaryRepository.Get().LastOrDefault();
                    DictionaryList.Add(lastDictionary);
                    OnPropertyChanged(nameof(DictionaryList));
                    if (lastDictionary != null)
                        await NavigationService.NavigateToAsync<WordsListViewModel>(lastDictionary);
                }
            }
            catch(Exception e)
            {
                Log.Logger.Error(e);
            }                        
        }

        private async void LoadData()
        {
            try
            {
                var list = new ObservableCollection<Dictionary>();
                //кроме словарей не законченных и словарей недоученных
                var items = await Task.Run(()=> _unitOfWork.DictionaryRepository.Get().Where(x => !x.Name.EndsWith(Constants.NAME_DB_FOR_CONTINUE, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.Name).AsEnumerable());
                for (int i = 0; i < items.Count(); i++)
                    list.Add(items.ElementAt(i));
                DictionaryList = list;
            }
            catch(Exception e)
            {
                Log.Logger.Error(e);               
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
            }
            catch(Exception e)
            {
                Log.Logger.Error(e);
            }
        }       
    }
}
