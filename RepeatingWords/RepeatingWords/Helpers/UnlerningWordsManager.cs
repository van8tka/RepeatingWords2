using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Heleprs;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepeatingWords.Helpers
{
    public class UnlerningWordsManager:IUnlearningWordsManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDictionaryNameLearningCreator _dictionaryNameCreator;
        public UnlerningWordsManager(IUnitOfWork unitOfWork, IDictionaryNameLearningCreator nameLearningCreator)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _dictionaryNameCreator = nameLearningCreator ?? throw new ArgumentNullException(nameof(nameLearningCreator));
        }

        public int SaveUnlearningDictionary(string nameDictionary, IEnumerable<Words> wordsUnlearn, IEnumerable<Words> wordsLeft, IEnumerable<Words> wordsAll)
        {
            try
            {
                var nameDictionaryNotLearning = _dictionaryNameCreator.CreateNameNotLearningDictionary(nameDictionary);
                var existNotLearningDictionary = _unitOfWork.DictionaryRepository.Get().Where(x => x.Name.Equals(nameDictionaryNotLearning)).FirstOrDefault();

                if ( (IsUnlearningDictionary(nameDictionary) && existNotLearningDictionary != null))
                {
                    //тогда удаляем из словаря выученные слова, если словарь пуст удаляем словарь
                    RemoveLearningWords(wordsUnlearn, wordsLeft, wordsAll);
                    RemoveDictionary(existNotLearningDictionary);
                    return existNotLearningDictionary.Id;
                }
                 else if(IsContinueWithUnlearningDictionary(nameDictionary) && existNotLearningDictionary != null)
                {
                    //тогда удаляем из словаря выученные слова, если словарь пуст удаляем словарь
                    var allWordsExistDict = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == existNotLearningDictionary.Id).AsEnumerable();
                    RemoveLearningWordsFromExistDictionary(wordsUnlearn, wordsLeft, allWordsExistDict, wordsAll);
                    RemoveDictionary(existNotLearningDictionary);
                    return existNotLearningDictionary.Id;
                }
                else if ( IsContinueDictionary(nameDictionary) && existNotLearningDictionary!=null )
                {
                    //тогда к словарю невыученных слов добавляем еще невыученные
                  return AddedUnlearningWords(existNotLearningDictionary, wordsUnlearn);
                }              
                
                else if (existNotLearningDictionary == null)
                {
                    //тогда создаем новый словарь невыученных слов
                    return CreateNewUnlearningDictionary(nameDictionaryNotLearning, wordsUnlearn);
                }
                else
                {//если начали учить заново, удаляем имеющиеся невыученные и создаем новый словарь невыученных слов
                    RemoveUnlearningWords(existNotLearningDictionary);
                    RemoveDictionary(existNotLearningDictionary);
                    return CreateNewUnlearningDictionary(nameDictionaryNotLearning, wordsUnlearn);
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                return -1;
            }
        }

        private void RemoveLearningWordsFromExistDictionary(IEnumerable<Words> wordsUnlearn, IEnumerable<Words> wordsLeft, IEnumerable<Words> allWords, IEnumerable<Words> continueAllWords)
        {
            var tempWords = new List<Words>(wordsLeft);
            tempWords.AddRange(wordsUnlearn);
            bool isDeleted = false;
            for (int i=0;i<allWords.Count();i++)
            {
                if(continueAllWords.Any(x=>x.RusWord.Equals(allWords.ElementAt(i).RusWord, StringComparison.OrdinalIgnoreCase)) && !tempWords.Any(x => x.RusWord.Equals(allWords.ElementAt(i).RusWord, StringComparison.OrdinalIgnoreCase)))
                {
                    _unitOfWork.WordsRepository.Delete(allWords.ElementAt(i));
                    isDeleted = true;
                }
            }
            if (isDeleted)
                _unitOfWork.Save();
        }

        private void RemoveUnlearningWords(Dictionary existNotLearningDictionary)
        {
            var allWords = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == existNotLearningDictionary.Id).AsEnumerable();
            for(int i=0;i<allWords.Count();i++)
            {
                _unitOfWork.WordsRepository.Delete(allWords.ElementAt(i));
            }
            _unitOfWork.Save();
        }

        private void RemoveLearningWords(IEnumerable<Words> wordsUnlearn, IEnumerable<Words> wordsLeft, IEnumerable<Words> wordsAll)
        {
            try
            {
                var tempWords = new List<Words>(wordsLeft);
                tempWords.AddRange(wordsUnlearn);
                bool isDeleted = false;
                for (int i = 0; i < wordsAll.Count(); i++)
                {
                    if (!tempWords.Contains(wordsAll.ElementAt(i)))
                    {
                        _unitOfWork.WordsRepository.Delete(wordsAll.ElementAt(i));
                        isDeleted = true;
                    }
                }
                if (isDeleted)
                    _unitOfWork.Save();
            }
            catch(Exception e)
            {
                throw;
            }           
        }

        private void RemoveDictionary(Dictionary dictionary)
        {
            var count = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == dictionary.Id)?.Count();
            if( count!=null && count == 0)
            {
                _unitOfWork.DictionaryRepository.Delete(dictionary);
                _unitOfWork.Save();
            }
        }

        private int AddedUnlearningWords(Dictionary existNotLearningDictionary, IEnumerable<Words> words)
        {
            try
            {             
                CreateWords(words, existNotLearningDictionary.Id);
                return existNotLearningDictionary.Id;
            }
            catch(Exception e)
            {
                throw;
            }            
        }

        private int CreateNewUnlearningDictionary(string nameDictionaryNotLearning, IEnumerable<Words> words)
        {
            try
            {
                int newIdDictionary = _unitOfWork.DictionaryRepository.Get().LastOrDefault().Id + 1;
                var unlerningDictionary = new Dictionary() { Id = newIdDictionary, Name = nameDictionaryNotLearning };
                _unitOfWork.DictionaryRepository.Create(unlerningDictionary);
                _unitOfWork.Save();
                CreateWords(words, newIdDictionary);
                return newIdDictionary;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private void CreateWords(IEnumerable<Words> words, int newIdDictionary)
        {
            try
            {
                for (int i = 0; i < words.Count(); i++)
                {
                    _unitOfWork.WordsRepository.Create(new Words()
                    {
                        Id = 0,
                        IdDictionary = newIdDictionary,
                        EngWord = words.ElementAt(i).EngWord,
                        RusWord = words.ElementAt(i).RusWord,
                        Transcription = words.ElementAt(i).Transcription
                    });
                }
                _unitOfWork.Save();
            }
            catch (Exception e)
            {
                throw;
            }           
        }

        private bool IsUnlearningDictionary(string nameDictionary) => nameDictionary.EndsWith(Resource.NotLearningPostfics);
        
        private bool IsContinueDictionary(string nameDictionary) => nameDictionary.EndsWith(Constants.NAME_DB_FOR_CONTINUE);

        private bool IsContinueWithUnlearningDictionary(string nameDictionary) => nameDictionary.EndsWith(Resource.NotLearningPostfics+Constants.NAME_DB_FOR_CONTINUE);
    }
}
