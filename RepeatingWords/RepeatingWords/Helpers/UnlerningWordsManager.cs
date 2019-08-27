using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
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
        public UnlerningWordsManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public int SaveDictionary(string nameDictionary, IEnumerable<Words> words)
        {
            try
            {
                var getLastDictionary = _unitOfWork.DictionaryRepository.Get().LastOrDefault();
                CheckAndRemoveDictionary(nameDictionary);
                int contunueID = getLastDictionary.Id + 1;
                var continueDictionary = new Dictionary() { Id = contunueID, Name = nameDictionary };
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

        public void CreateLastAction(int contunueID, bool isFromNative)
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

        private void CheckAndRemoveDictionary(string nameContinueDictionary)
        {
            try
            {
                var continueDictionary = _unitOfWork.DictionaryRepository.Get().Where(x => x.Name.Equals(nameContinueDictionary, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (continueDictionary != null)
                {
                    var words = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == continueDictionary.Id);
                    foreach (var word in words)
                    {
                        _unitOfWork.WordsRepository.Delete(word);
                    }
                    _unitOfWork.DictionaryRepository.Delete(continueDictionary);
                    _unitOfWork.Save();
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error(e);
                throw;
            }
        }
    }
}
