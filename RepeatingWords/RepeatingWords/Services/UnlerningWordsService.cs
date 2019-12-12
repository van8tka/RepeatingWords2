using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RepeatingWords.Service
{
    public class UnlerningWordsService : IUnlearningWordsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDictionaryNameLearningCreator _dictionaryNameCreator;
        private readonly IDictionaryTypeByName _typeDictionary;
        private bool _isCreatedNew;
        private IList<Words> _addedWords;
        public UnlerningWordsService(IUnitOfWork unitOfWork, IDictionaryNameLearningCreator nameLearningCreator, IDictionaryTypeByName typeDictionary)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _dictionaryNameCreator = nameLearningCreator ?? throw new ArgumentNullException(nameof(nameLearningCreator));
            _typeDictionary = typeDictionary ?? throw new ArgumentNullException(nameof(typeDictionary));
            _isCreatedNew = false;
            _addedWords = new List<Words>();
        }


       

        public void CheckSaveOrRemoveWord(Words word, bool isSaveWord, string nameDictionary)
        {
            if (isSaveWord && !_typeDictionary.IsUnlearningDictionary(nameDictionary) && !_typeDictionary.IsContinueWithUnlearningDictionary(nameDictionary))
            {
                if (_addedWords.Contains(word))
                    return;
                SaveUnlearningDictionary(nameDictionary, word);
                _addedWords.Add(word);
            }               
            else if (!isSaveWord && (_typeDictionary.IsUnlearningDictionary(nameDictionary) || _typeDictionary.IsContinueWithUnlearningDictionary(nameDictionary)))
                 RemoveUnlearningDictionary(nameDictionary, word);
        }


        /// <summary>
        /// удаление слов из словаря невыученных слов в двух случаях:
        /// 1. изучение невыученных слов 
        /// 2. продолжение изучения невыученных слов
        /// </summary>
        /// <param name="nameDictionary">имя словаря загруженного для изучения</param>
        /// <param name="word">слово для удаления</param>
        /// <returns></returns>
        public void RemoveUnlearningDictionary(string nameDictionary, Words word)
        {           
                try
                {              
                    var nameDictionaryNotLearning = _dictionaryNameCreator.CreateNameNotLearningDictionary(nameDictionary);
                    var existNotLearningDictionary = _unitOfWork.DictionaryRepository.Get().Where(x => x.Name.Equals(nameDictionaryNotLearning)).FirstOrDefault();
                    if ((_typeDictionary.IsUnlearningDictionary(nameDictionary) || _typeDictionary.IsContinueWithUnlearningDictionary(nameDictionary)) && existNotLearningDictionary != null)
                    {
                        //тогда удаляем из словаря выученные слова, если словарь пуст удаляем словарь
                        RemoveLearningWords(word, existNotLearningDictionary);
                        RemoveDictionary(existNotLearningDictionary);
                    }
                }
                catch (Exception e)
                {
                    Log.Logger.Error(e);
                }          
        }
     
        /// <summary>
        /// добавление слова в словарь не выученных слов 
        /// 1.в начале изучения 
        /// 2. при продолжении изучения 
        /// 3. в случае нового изучения уже изучавшегося словаря.
        /// Создание словаря не выученных слов при его отсутствии
        /// </summary> 
        /// <param name="nameDictionary">имя словаря загруженного для изучения</param>
        /// <param name="word">не выученное слово которое необходимо добавить в словарь</param>
        /// <returns></returns>
        public void SaveUnlearningDictionary(string nameDictionary, Words word)
        {
            
                try
                {
                    var nameDictionaryNotLearning = _dictionaryNameCreator.CreateNameNotLearningDictionary(nameDictionary);
                    var existNotLearningDictionary = _unitOfWork.DictionaryRepository.Get().Where(x => x.Name.Equals(nameDictionaryNotLearning)).FirstOrDefault();
                    if (_typeDictionary.IsContinueDictionary(nameDictionary) && existNotLearningDictionary != null)
                    {
                        //тогда к словарю невыученных слов добавляем еще невыученные
                        CreateWords(word, existNotLearningDictionary.Id);
                    }
                    else if (existNotLearningDictionary == null)
                    {
                        //тогда создаем новый словарь невыученных слов
                        CreateNewUnlearningDictionary(nameDictionaryNotLearning, word);
                        _isCreatedNew = true;
                    }
                    else
                    {//если начали учить заново, удаляем имеющиеся невыученные и создаем новый словарь невыученных слов
                        if(!_isCreatedNew)
                        {
                            RemoveUnlearningWords(existNotLearningDictionary);
                            RemoveDictionary(existNotLearningDictionary);
                            CreateNewUnlearningDictionary(nameDictionaryNotLearning, word);
                            _isCreatedNew = true;
                        }
                        CreateWords(word, existNotLearningDictionary.Id);
                    }
                }
                catch (Exception e)
                {
                    Log.Logger.Error(e);
                    Debugger.Break();
                }            
        }

        private void RemoveUnlearningWords(Dictionary existNotLearningDictionary)
        {
            var allWords = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == existNotLearningDictionary.Id).AsEnumerable();
            for (int i = 0; i < allWords.Count(); i++)
            {
                _unitOfWork.WordsRepository.Delete(allWords.ElementAt(i));
            }
            _unitOfWork.Save();
        }


        private void RemoveLearningWords(Words word, Dictionary dictionary)
        {
            try
            {
                var delWord = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == dictionary.Id && x.RusWord.Equals(word.RusWord, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if(delWord!=null)
                {
                    _unitOfWork.WordsRepository.Delete(delWord);
                    _unitOfWork.Save();
                }              
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

 
        private void RemoveDictionary(Dictionary dictionary)
        {
            var count = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == dictionary.Id)?.Count();
            if (count != null && count == 0)
            {
                _unitOfWork.DictionaryRepository.Delete(dictionary);
                _unitOfWork.Save();
            }
        }

  
        private int CreateNewUnlearningDictionary(string nameDictionaryNotLearning, Words words)
        {
            try
            {
                int newIdDictionary = _unitOfWork.DictionaryRepository.Get().LastOrDefault().Id + 1;
                var unlerningDictionary = new Dictionary() { Id = newIdDictionary, Name = nameDictionaryNotLearning, PercentOfLearned = 0, LastUpdated = DateTime.UtcNow};
                _unitOfWork.DictionaryRepository.Create(unlerningDictionary);
                _unitOfWork.Save();
                CreateWords(words, newIdDictionary);
                return newIdDictionary;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }


        private void CreateWords(Words word, int newIdDictionary)
        {
            try
            {
                _unitOfWork.WordsRepository.Create(new Words()
                {
                    Id = 0,
                    IdDictionary = newIdDictionary,
                    EngWord = word.EngWord,
                    RusWord = word.RusWord,
                    Transcription = word.Transcription
                });
                _unitOfWork.Save();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }       
    }
}
