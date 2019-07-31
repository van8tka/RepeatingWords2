using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RepeatingWords.ViewModel
{
    public class DictionaryFrNetViewModel : ViewModelBase
    {
        private readonly IWebApiService _webService;
        private readonly IUnitOfWork _unitOfWork;
        public DictionaryFrNetViewModel(INavigationService navigationServcie, IDialogService dialogService, IWebApiService webService, IUnitOfWork unitOfWork) : base(navigationServcie, dialogService)
        {
            _webService = webService;
            _unitOfWork = unitOfWork;
            DictionaryList = new ObservableCollection<Dictionary>();           
        }
       

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            if (navigationData is Language language)
            {
                LanguageName = language.NameLanguage;
                await LoadData(language.Id);
            }              
            else
                Debugger.Break();
           await base.InitializeAsync(navigationData);
        }

        private async Task LoadData(int idLanguage)
        {
            try
            {
                IEnumerable<Dictionary> data = (await _webService.GetLanguage(idLanguage))?.OrderBy(x => x.Name);
                if (data != null)
                {
                    var list = new ObservableCollection<Dictionary>();
                    for (int i = 0; i < data.Count(); i++)
                        list.Add(data.ElementAt(i));
                    DictionaryList = list;
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }

        private string _languageName;
        public string LanguageName { get => _languageName; set { _languageName = value; OnPropertyChanged(nameof(LanguageName)); } }

        private ObservableCollection<Dictionary> _dictionaryList;
        public ObservableCollection<Dictionary> DictionaryList { get => _dictionaryList; set { _dictionaryList = value; OnPropertyChanged(nameof(DictionaryList)); } }

        private Dictionary _selectedItem;
        public Dictionary SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                if (_selectedItem != null)
                    AddDictionaryToDb(_selectedItem);
            }
        }

        private async void AddDictionaryToDb(Dictionary selectedDictionary)
        {
            try
            {
                SelectedItem = null;
                var words = (await _webService.Get(selectedDictionary.Id)).OrderBy(x => x.RusWord);
                int idNewDictionary = _unitOfWork.DictionaryRepository.Get().Last().Id + 1;
                var dictionary = _unitOfWork.DictionaryRepository.Create(new Dictionary() { Id = idNewDictionary, Name = selectedDictionary.Name });
                _unitOfWork.Save();
                CreateWords(words, idNewDictionary);
                _unitOfWork.Save();
                await NavigationService.NavigateToAsync<WordsListViewModel>(dictionary);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
            }
        }

        private void CreateWords(IEnumerable<Words> words, int idNewDictionary)
        {
            var badSymbals = new char[] { ' ', '\r', '\n', '\t' };
            for (int i = 0; i < words.Count(); i++)
            {
                var newWord = new Words();
                var netWord = words.ElementAt(i);
                newWord.Id = 0;
                newWord.IdDictionary = idNewDictionary;
                newWord.RusWord = netWord.RusWord.Trim(badSymbals);
                newWord.Transcription = netWord.Transcription.Trim(badSymbals);
                newWord.EngWord = netWord.EngWord.Trim(badSymbals);
                _unitOfWork.WordsRepository.Create(newWord);
            }
        }
    }
}
