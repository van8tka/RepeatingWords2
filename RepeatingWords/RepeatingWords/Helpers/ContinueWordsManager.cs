using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.LoggerService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RepeatingWords.Helpers
{
    public class ContinueWordsManager : IContinueWordsManager 
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDictionaryNameLearningCreator _dictionaryNameCreator;
        public ContinueWordsManager(IUnitOfWork unitOfWork, IDictionaryNameLearningCreator dictionaryNameCreator)
        {
            _unitOfWork = unitOfWork;
            _dictionaryNameCreator = dictionaryNameCreator;
        }

        public bool RemoveContinueDictionary()
        {
            try
            {
                var lastAction = _unitOfWork.LastActionRepository.Get().LastOrDefault();
                if (lastAction != null)
                {
                    var dict = _unitOfWork.DictionaryRepository.Get(lastAction.IdDictionary);
                    _unitOfWork.DictionaryRepository.Delete(dict);
                    var words = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == lastAction.IdDictionary).AsEnumerable();
                    for (int i = 0; i < words.Count(); i++)
                    {
                        _unitOfWork.WordsRepository.Delete(words.ElementAt(i));
                    }
                    _unitOfWork.LastActionRepository.Delete(lastAction);
                    _unitOfWork.Save();
                }
                return true;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                return false;
            }
        }

        public int SaveContinueDictionary(string nameDictionary, IList<Words> words, bool isFromNative)
        {
            try
            {
                RemoveContinueDictionary();
                var nameContinueDictionary = _dictionaryNameCreator.CreateNameContinueDictionary(nameDictionary);
                int newDictionaryId =  SaveDictionary(nameContinueDictionary, words);
                CreateLastAction(newDictionaryId, isFromNative);
                return newDictionaryId;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                return -1;
            }
        }

        private int SaveDictionary(string nameContinueDictionary, IList<Words> words)
        {
            try
            {
                var getLastDictionary = _unitOfWork.DictionaryRepository.Get().LastOrDefault();               
                int contunueID = getLastDictionary.Id + 1;
                var continueDictionary = new Dictionary() { Id = contunueID, Name = nameContinueDictionary };
                _unitOfWork.DictionaryRepository.Create(continueDictionary);
                _unitOfWork.Save();
                foreach (var word in words)
                {
                    _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = contunueID, EngWord = word.EngWord, RusWord = word.RusWord, Transcription = word.Transcription });
                }
                _unitOfWork.Save();
                return contunueID;
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }

        private void CreateLastAction(int contunueID, bool isFromNative)
        {
            var lastAction = _unitOfWork.LastActionRepository.Get().LastOrDefault();
            if (lastAction == null)
            {
                lastAction = new LastAction() { Id = 0, FromRus = isFromNative, IdDictionary = contunueID, IdWord = 0 };
                _unitOfWork.LastActionRepository.Create(lastAction);
            }
            else
            {
                lastAction.IdDictionary = contunueID;
                lastAction.FromRus = isFromNative;
                _unitOfWork.LastActionRepository.Update(lastAction);
            }
            _unitOfWork.Save();
        }

    }
}
