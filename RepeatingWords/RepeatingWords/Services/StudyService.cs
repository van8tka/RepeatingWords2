using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Heleprs;
using RepeatingWords.Interfaces;
using RepeatingWords.LoggerService;
using RepeatingWords.Model;

namespace RepeatingWords.Services
{
    public interface ILanguageStudyService{
        ObservableCollection<LanguageModel> DictionaryList { get; }
        Language GetLanguage(string name);
        int AddLanguage(string nameLanguage);
        int AddLanguage(Language language);
        bool RemoveLanguage(int idlanguage);
    }
    public interface IWordStudyService
    {
        WordsModel AddWord(WordsModel word);
        bool UpdateWord(WordsModel word);
        void ResetStudiedWords(int iddictonary);
        void RemoveWord(WordsModel selectedItem);
        void AddWords(IEnumerable<Words> listWords);
    }
    public interface IDictionaryStudyService
    {
        DictionaryModel GetDictionary(int idDictionary);
        bool RemoveDictionaryFromLanguage(int dictionaryId);
        int AddDictionary(string dictionaryName, int idLang);
        int AddDictionary(Dictionary dictionary);
        void UpdateDictionary(DictionaryModel dictionary);
    }
    public interface ITransactionService
    {
        void BeginTransaction();
        void CommitTransaction();
        void RollBackTransaction();
    }

    public interface IStudyService : ILanguageStudyService, ITransactionService, IWordStudyService, IDictionaryStudyService
    {
         bool ClearDB();
    }


    public class StudyService : IStudyService
    {
        public StudyService(IUnitOfWork unitOfWork, IInitDefaultDb initDb)
        {
            _unitOfWork = unitOfWork;
            _initDb = initDb;
            InitData();
        }


        public void BeginTransaction()
        {
            _unitOfWork.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _unitOfWork.CommitTransaction();
        }

        public void RollBackTransaction()
        {
            _unitOfWork.RollBackTransaction();
        }

        private void InitData()
        {
            _loadDataTask = Task.Run(() =>
            {
                _initDb.LoadDefaultData();
                _languages = _unitOfWork.LanguageRepository.Get().AsEnumerable().ToList();
                _dictionaries = _unitOfWork.DictionaryRepository.Get().AsEnumerable().ToList();
                _words = _unitOfWork.WordsRepository.Get().AsEnumerable().ToList();
            });
        }

        private Task _loadDataTask;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInitDefaultDb _initDb;

        private IList<Language> _languages;
        private IList<Dictionary> _dictionaries;
        private IList<Words> _words;

        private ObservableCollection<LanguageModel> _dictionaryList;

        public ObservableCollection<LanguageModel> DictionaryList
        {
            get => _dictionaryList ?? (_dictionaryList = getDictionaryList());
        }

        public DictionaryModel GetDictionary(int idDictionary)
        {
            var idLang = _dictionaries.FirstOrDefault(x => x.Id == idDictionary)?.IdLanguage;
            return DictionaryList.FirstOrDefault(x => x.Id == idLang)?.FirstOrDefault(x => x.Id == idDictionary);
        }

        private ObservableCollection<LanguageModel> getDictionaryList()
        {
            try
            {
                var dictionaryList = new ObservableCollection<LanguageModel>();
                _loadDataTask.Wait();
                foreach (var lang in _languages)
                {
                    var dictionariesDb = _dictionaries.Where(x => x.IdLanguage == lang.Id)
                        .OrderByDescending(x => x.LastUpdated).AsEnumerable();
                    if (dictionariesDb != null && dictionariesDb.Any())
                    {
                        int countDictionary = dictionariesDb.Count();
                        List<DictionaryModel> dictModels = new List<DictionaryModel>(countDictionary);
                        for (int i = 0; i < countDictionary; i++)
                        {
                            var dict = dictionariesDb.ElementAt(i);
                            var words = _words.Where(x => x.IdDictionary == dict.Id);
                            dictModels.Add(new DictionaryModel(dict, words));
                        }

                        var langModel = new LanguageModel(lang, dictModels, false);
                        dictionaryList.Add(langModel);
                    }

                }

                return dictionaryList;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }

        public int AddLanguage(Language language)
        {
            try
            {
                var languageNew = _unitOfWork.LanguageRepository.Create(language);
                _unitOfWork.Save();
                _languages.Add(languageNew);
                _dictionaryList.Add(new LanguageModel(languageNew));
                return languageNew.Id;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }


        public int AddLanguage(string nameLanguage)
        {
            try
            {
                if (!string.IsNullOrEmpty(nameLanguage) || !string.IsNullOrWhiteSpace(nameLanguage))
                {
                    var language = new Language() {Id = 0, NameLanguage = nameLanguage, PercentOfLearned = 0};
                    return AddLanguage(language);
                }

                return -1;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }

        public bool RemoveLanguage(int idlanguage)
        {
            try
                {
                    var removedLanguage = _dictionaryList.FirstOrDefault(x => x.Id == idlanguage);
                    var dictionaries = _dictionaries.Where(x => x.IdLanguage == idlanguage).AsEnumerable();
                    int count = dictionaries.Count();
                    for (int i = count-1; i >=0 ; i--)
                    {
                        RemoveDictionary(dictionaries.ElementAtOrDefault(i).Id);
                    }
                    var language = _unitOfWork.LanguageRepository.Get(idlanguage);
                    _unitOfWork.LanguageRepository.Delete(language);
                    _unitOfWork.Save();
                    _languages.Remove(language);
                    _dictionaryList.Remove(removedLanguage);
                    return true;
                }
                catch (Exception e)
                {
                    Log.Logger.Error(e);
                    return false;
                }
        }

        public bool RemoveDictionaryFromLanguage(int dictionaryId)
        {
            try
            {
                return RemoveDictionary(dictionaryId);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                return false;
            }
        }

        private bool RemoveDictionary(int dictionaryId)
        {
            try
            {
                var words = _words.Where(x => x.IdDictionary == dictionaryId).AsEnumerable();
                int count = words.Count();
                for (int i = count-1; i >= 0; i--)
                {
                    var word = words.ElementAtOrDefault(i);
                    _unitOfWork.WordsRepository.Delete(word);
                    _words.Remove(word);
                }
                var dictionary = _dictionaries.FirstOrDefault(x => x.Id == dictionaryId);
                bool success = _unitOfWork.DictionaryRepository.Delete(dictionary);
                _dictionaries.Remove(dictionary);
                _unitOfWork.Save();
                return true;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw ;
            }
        }

        public int AddDictionary(Dictionary dictionary)
        {
            try
            {
                var dictionaryNew = _unitOfWork.DictionaryRepository.Create(dictionary);
                _unitOfWork.Save();
                _dictionaries.Add(dictionaryNew);
                var dictModel = new DictionaryModel(dictionaryNew, new List<Words>());
                _dictionaryList.FirstOrDefault(x => x.Id == dictionaryNew.IdLanguage).AddDictionary(dictModel);
                return dictionaryNew.Id;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }

        public int AddDictionary(string dictionaryName, int idLang)
        {
            try
            {
                if (!string.IsNullOrEmpty(dictionaryName) || !string.IsNullOrWhiteSpace(dictionaryName))
                {
                    var dictionary = new Dictionary()
                    {
                        Id = 0, IdLanguage = idLang, Name = dictionaryName, PercentOfLearned = 0,
                        LastUpdated = DateTime.UtcNow
                    };
                    return AddDictionary(dictionary);
                }

                return -1;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }


        public void AddWords(IEnumerable<Words> listWords)
        {
            _unitOfWork.WordsRepository.Create(listWords);
            _unitOfWork.Save();
            (_words as List<Words>)?.AddRange(listWords);
            var dictModel = GetDictionary(listWords.FirstOrDefault().IdDictionary);
            for (int i = 0; i < listWords.Count(); i++)
            {
                dictModel.WordsCollection.Add(new WordsModel(dictModel, listWords.ElementAt(i)));
            }
        }

        public WordsModel AddWord(WordsModel wordNew)
        {
            try
            {
                var word = new Words();
                word.Id = wordNew.Id;
                word.RusWord = wordNew.RusWord;
                word.EngWord = wordNew.EngWord;
                word.Transcription = wordNew.Transcription;
                word.IsLearned = wordNew.IsLearned;
                word.IdDictionary = wordNew.DictionaryParent.Id;
                var wordDb = _unitOfWork.WordsRepository.Create(word);
                SetDictionaryUpdate(word.IdDictionary);
                _words.Add(word);
                wordNew.Id = wordDb.Id;
                return wordNew;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }

        public void UpdateDictionary(DictionaryModel dictionary)
        {
            Log.Logger.Info($"\n Update dictionary id={dictionary.Id}; name={dictionary.Name}");
            SetDictionaryUpdate(dictionary.Id);
        }

        private void SetDictionaryUpdate(int idDictionary)
        {
            var dictionary = GetDictionary(idDictionary);
            dictionary.LastUpdated = DateTime.UtcNow;
            var dbDictionary = _dictionaries.FirstOrDefault(x => x.Id == idDictionary);
            int perc;
            if (int.TryParse(dictionary.PercentOfLearned.TrimEnd('%'), out perc))
                dbDictionary.PercentOfLearned = perc;
            dbDictionary.LastUpdated = dictionary.LastUpdated;
            _unitOfWork.DictionaryRepository.Update(dbDictionary);
            _unitOfWork.Save();
        }

        public bool UpdateWord(WordsModel word)
        {
            try
            {
                var wordDb = _words.FirstOrDefault(x => x.Id == word.Id);
                int index = _words.IndexOf(wordDb);
                _words.ElementAt(index).EngWord = word.EngWord;
                _words.ElementAt(index).IsLearned = word.IsLearned;
                _words.ElementAt(index).RusWord = word.RusWord;
                _words.ElementAt(index).Transcription = word.Transcription;
                wordDb.EngWord = word.EngWord;
                wordDb.IsLearned = word.IsLearned;
                wordDb.RusWord = word.RusWord;
                wordDb.Transcription = word.Transcription;
                _unitOfWork.WordsRepository.Update(wordDb);
                SetDictionaryUpdate(word.DictionaryParent.Id);
                return true;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }

        public void ResetStudiedWords(int iddictonary)
        {
            Log.Logger.Info($"\n Reset studies words in idDictionary={iddictonary}");
            var words = _words.Where(x => x.IdDictionary == iddictonary && x.IsLearned);
            Parallel.ForEach(words, (w) => { w.IsLearned = false; });
            _unitOfWork.WordsRepository.Update(words);
            SetDictionaryUpdate(iddictonary);
        }

        public void RemoveWord(WordsModel word)
        {
            var wordDb = _words.FirstOrDefault(x => x.Id == word.Id);
            _unitOfWork.WordsRepository.Delete(wordDb);
            _unitOfWork.Save();
        }

        public Language GetLanguage(string name)
        {
            return _languages.FirstOrDefault(x => x.NameLanguage.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public bool ClearDB()
        {
            try
            { 
                Log.Logger.Info(Environment.NewLine + "Clear DB");
                BeginTransaction();
                int count = _words.Count;
                for (int i = count-1; i >=0; i--)
                {
                    _unitOfWork.WordsRepository.Delete(_words.ElementAt(i));
                }
                count = _dictionaries.Count;
                for (int i = count-1; i >= 0; i--)
                {
                    _unitOfWork.DictionaryRepository.Delete(_dictionaries.ElementAt(i));
                }
                count = _languages.Count;
                for (int i = count-1; i >= 0; i--)
                {
                    _unitOfWork.LanguageRepository.Delete(_languages.ElementAt(i));
                }
                CommitTransaction();
                ResetLocalData();
                return true;
            }
            catch (Exception e)
            {
                RollBackTransaction();
                Log.Logger.Error(e);
                return false;
            }
        }

        private void ResetLocalData()
        {
            _words.Clear();
            _dictionaries.Clear();
            _languages.Clear();
            _dictionaryList.Clear();
        }
    }
}
