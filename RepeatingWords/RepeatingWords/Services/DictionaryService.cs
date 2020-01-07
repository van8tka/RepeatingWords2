using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.Model;

namespace RepeatingWords.Services
{
    public interface IDictionaryStudyService
    {
        ObservableCollection<LanguageModel> DictionaryList { get; }
        Dictionary GetDictionary(int idDictionary);
        Dictionary GetUnlearningDictionary(string nameDictionary);
        int AddLanguage(string nameLanguage);
        Task<bool> RemoveLanguage(int idlanguage);
        Task<bool> RemoveDictionaryFromLanguage(Dictionary dictionary, LanguageModel removedLanguage);
        int AddDictionary(string dictionaryName, int idLang);
        void RemoveWord(Words selectedItem);
        IEnumerable<Words> GetWordsByDictionary(int id);
    }





   public class DictionaryStudyService:IDictionaryStudyService
    {
        public DictionaryStudyService(IUnitOfWork unitOfWork, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _unitOfWork = unitOfWork;
            InitData();
        }

        private void InitData()
        {
            //_loadDataTask = Task.Run(() =>
            //{
                _languages = _unitOfWork.LanguageRepository.Get().AsEnumerable().ToList();
                _dictionaries = _unitOfWork.DictionaryRepository.Get().AsEnumerable().ToList();
            _words = _unitOfWork.WordsRepository.Get().AsEnumerable().ToList();
            // });
        }

      //  private Task _loadDataTask;
        private readonly IDialogService _dialogService;
        private readonly IUnitOfWork _unitOfWork;

        private IList<Language> _languages;
        private IList<Dictionary> _dictionaries;
        private IList<Words> _words;

        private ObservableCollection<LanguageModel> _dictionaryList;
        public ObservableCollection<LanguageModel> DictionaryList { get=>_dictionaryList??(_dictionaryList=getDictionaryList()); }
        public Dictionary GetDictionary(int idDictionary)
        {
            return _dictionaries.FirstOrDefault(x => x.Id == idDictionary);
        }

        private ObservableCollection<LanguageModel> getDictionaryList()
        {
            try
            {

                var dictionaryList = new ObservableCollection<LanguageModel>();
                //_loadDataTask.Wait();
                foreach (var lang in _languages)
                {
                    var items = _dictionaries.Where(x => x.IdLanguage == lang.Id && !x.Name.EndsWith(Constants.NAME_DB_FOR_CONTINUE, StringComparison.OrdinalIgnoreCase) && !x.Name.EndsWith(Resource.NotLearningPostfics, StringComparison.OrdinalIgnoreCase)).OrderByDescending(x => x.LastUpdated).AsEnumerable();
                    var langModel = new LanguageModel(lang, items, false);
                    dictionaryList.Add(langModel);
                }
                return dictionaryList;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                _dialogService.ShowToast("DictionaryService. Error load data!");
                return new ObservableCollection<LanguageModel>();
            }
        }

        public Dictionary GetUnlearningDictionary(string nameDictionary)
        {
            return _dictionaries.FirstOrDefault(x => x.Name.Equals(nameDictionary + Resource.NotLearningPostfics, StringComparison.OrdinalIgnoreCase));
        }


        public int AddLanguage(string nameLanguage)
        {
            try
            {
                if (!string.IsNullOrEmpty(nameLanguage) || !string.IsNullOrWhiteSpace(nameLanguage))
                {
                    var language = _unitOfWork.LanguageRepository.Create(new Language() { Id = 0, NameLanguage = nameLanguage, PercentOfLearned = 0 });
                    _unitOfWork.Save();
                    _languages.Add(language);
                    _dictionaryList.Add(new LanguageModel(language));
                    return language.Id;
                }
                return -1;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                _dialogService.ShowToast("DictionaryService. Error load language");
                return -1;
            }
        }

        public async Task<bool> RemoveLanguage(int idlanguage)
        {
            try
            {
                var removedLanguage = _dictionaryList.FirstOrDefault(x => x.Id == idlanguage);
                if (removedLanguage != null)
                {
                    var dictionaries = _unitOfWork.DictionaryRepository.Get().Where(x => x.IdLanguage == idlanguage).AsEnumerable();
                    foreach (var item in dictionaries)
                    {
                        await RemoveDictionaryFromLanguage(item, removedLanguage);
                    }
                    var language = _unitOfWork.LanguageRepository.Get(removedLanguage.Id);
                    _unitOfWork.LanguageRepository.Delete(language);
                    _unitOfWork.Save();
                    _dictionaryList.Remove(removedLanguage);
                    _languages.Remove(language);
                }
                return true;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                _dialogService.ShowToast("DictionaryService. Error remove language");
                return false;
            }
        }

        public async Task<bool> RemoveDictionaryFromLanguage(Dictionary dictionary, LanguageModel removedLanguage)
        {
            try
            {
                var unlearned = GetUnlearningDictionary(dictionary.Name);
                if (unlearned != null)
                    await RemoveDictionaryFromLanguage(unlearned, removedLanguage);
                var words = await Task.Run(() => _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == dictionary.Id).AsEnumerable());
                if (words != null && words.Any())
                    for (int i = 0; i < words.Count(); i++)
                    {
                        var word = words.ElementAt(i);
                        await Task.Run(() => _unitOfWork.WordsRepository.Delete(word));
                        _words.Remove(word);
                    }
                bool success = await Task.Run(() => _unitOfWork.DictionaryRepository.Delete(dictionary));
                _dictionaries.Remove(dictionary);
                _unitOfWork.Save();
                removedLanguage.RemoveDictionary(dictionary);
                return true;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                _dialogService.ShowToast("DictionaryService. Error remove dictionary");
                throw;
            }
        }

        public int AddDictionary(string dictionaryName, int idLang)
        {
            try
            {
                if (!string.IsNullOrEmpty(dictionaryName) || !string.IsNullOrWhiteSpace(dictionaryName))
                {
                    var dictionary = _unitOfWork.DictionaryRepository.Create(new Dictionary() { Id = 0, IdLanguage = idLang, Name = dictionaryName, PercentOfLearned = 0, LastUpdated = DateTime.UtcNow });
                    _unitOfWork.Save();
                    return  _dictionaryList.FirstOrDefault(x=>x.Id == idLang).AddDictionary(dictionary).Id;
                }
                return -1;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                _dialogService.ShowToast("DictionaryService. Error add dictionary");
                return -1;
            }
        }

        public void RemoveWord(Words word)
        {
            try
            {
                _unitOfWork.WordsRepository.Delete(word);
                _unitOfWork.Save();
                _words.Remove(word);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                _dialogService.ShowToast("DictionaryService. Error remove word");
            }
        }

        public IEnumerable<Words> GetWordsByDictionary(int id)
        {
           return _words.Where(x => x.IdDictionary == id).OrderBy(x => x.RusWord).AsEnumerable();
        }
    }
}
