using NUnit.Framework;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity;

namespace RepeatingWords.NUnitTest
{
    [TestFixture]
   public class TestUnlearningManager
    {
        private IUnlearningWordsService _unlearningWordsManager;
        private IUnitOfWork _unitOfWork;
        private Dictionary _defaultDictionary;
        private IEnumerable<Words> _wordsAllDefaultCollection;
        internal const string NAME_DB_FOR_CONTINUE = "ContinueDictionary";

        public TestUnlearningManager()
        {
            var container = UnityConfig.Load();           
            _unitOfWork = container.Resolve<IUnitOfWork>();         
            _unlearningWordsManager = container.Resolve<IUnlearningWordsService>();          
            var init = container.Resolve<IInitDefaultDb>();
            init.LoadDefaultData();
            _defaultDictionary = _unitOfWork.DictionaryRepository.Get().FirstOrDefault();
            _wordsAllDefaultCollection = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == _defaultDictionary.Id).AsEnumerable();
        }

  
         [TearDown]
        public void Reset()
        {
            var allDictionaries = _unitOfWork.DictionaryRepository.Get().AsEnumerable();
            foreach(var dictionary in allDictionaries)
            {
                if(!dictionary.Name.Equals(_defaultDictionary.Name, StringComparison.OrdinalIgnoreCase))
                {
                    var words = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == dictionary.Id).AsEnumerable();
                    for (int i = 0; i < words.Count(); i++)
                        _unitOfWork.WordsRepository.Delete(words.ElementAt(i));
                    _unitOfWork.DictionaryRepository.Delete(dictionary);
                    _unitOfWork.Save();
                }              
            }
        }


        /// <summary>
        /// при изучении слов, неизученные слова добавляются в словарь невыученных слов
        /// </summary>
        [Test]
        public void SaveUnlearningDictionary_UnExistUnlearnedDictionary_IdDictionary()
        {   
            //added 1 word
            var wordsUnlearn = _wordsAllDefaultCollection.FirstOrDefault();
            //act
            _unlearningWordsManager.SaveUnlearningDictionary(_defaultDictionary.Name, wordsUnlearn).GetAwaiter().GetResult();
            //assert
            var dictionaries = _unitOfWork.DictionaryRepository.Get();
            var newDictionary = dictionaries.Where(x => x.Name.Equals(_defaultDictionary.Name + Resource.NotLearningPostfics, StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual(2, dictionaries.Count());
            Assert.IsTrue(newDictionary.Any());
            int countWords = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == newDictionary.FirstOrDefault().Id).Count();
            Assert.AreEqual(1,countWords);
           
            //added second word
            wordsUnlearn = _wordsAllDefaultCollection.Skip(1).Take(1).FirstOrDefault();
            //act
            _unlearningWordsManager.SaveUnlearningDictionary(_defaultDictionary.Name, wordsUnlearn).GetAwaiter().GetResult();
            //assert
            dictionaries = _unitOfWork.DictionaryRepository.Get();
            newDictionary = dictionaries.Where(x => x.Name.Equals(_defaultDictionary.Name + Resource.NotLearningPostfics, StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual(2, dictionaries.Count());          
            countWords = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == newDictionary.FirstOrDefault().Id).Count();
            Assert.AreEqual(2, countWords);
        }
        /// <summary>
        /// при продолжении изучении слов, неизученные слова добавляются в словарь невыученных слов
        /// </summary>
        [Test]
        public void SaveUnlearningDictionary_LearnContinueDictionary_WithEmptyUnlearningDictionary()
        {
            var nameContinueDictionary = _defaultDictionary.Name + NAME_DB_FOR_CONTINUE;
            var lastId = _unitOfWork.DictionaryRepository.Get().LastOrDefault().Id;
            var contUnlDic = _unitOfWork.DictionaryRepository.Create(new Dictionary() { Id = ++lastId, Name = nameContinueDictionary });                    
            _unitOfWork.Save();
            var word1 = _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = contUnlDic.Id, RusWord = "tost" });
             _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = contUnlDic.Id, RusWord = "best" });
            var word2 = _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = contUnlDic.Id, RusWord = "dron" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = contUnlDic.Id, RusWord = "step" });
            _unitOfWork.Save();         
            //act           
            _unlearningWordsManager.SaveUnlearningDictionary(nameContinueDictionary, word1).GetAwaiter().GetResult();
            //assert
            var dictionaries = _unitOfWork.DictionaryRepository.Get();
            var newDictionary = dictionaries.Where(x => x.Name.Equals(_defaultDictionary.Name + Resource.NotLearningPostfics, StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual(3, dictionaries.Count());
            Assert.IsTrue(newDictionary.Any());
            var words = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == newDictionary.FirstOrDefault().Id);
            Assert.AreEqual(1, words.Count());
            Assert.AreEqual(word1.RusWord, words.FirstOrDefault().RusWord);
            //add second word
            _unlearningWordsManager.SaveUnlearningDictionary(nameContinueDictionary, word2).GetAwaiter().GetResult();
            //assert
            dictionaries = _unitOfWork.DictionaryRepository.Get();            
            Assert.AreEqual(3, dictionaries.Count());            
            words = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == newDictionary.FirstOrDefault().Id);
            Assert.AreEqual(2, words.Count());
            Assert.AreEqual(word2.RusWord, words.LastOrDefault().RusWord);
        }

        /// <summary>
        /// при изучении неизученных ранее слов, слова удаляются по мере их изучения
        /// </summary>
        [Test]
        public void RemoveUnlearningDictionary_RemoveFromUnlearningDictionary()
        {
            var nameNotLearn = _defaultDictionary.Name + Resource.NotLearningPostfics;        
            var lastId = _unitOfWork.DictionaryRepository.Get().LastOrDefault().Id;
            var unlearnDic = _unitOfWork.DictionaryRepository.Create(new Dictionary() { Id = ++lastId, Name = nameNotLearn });
            _unitOfWork.Save();
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "fill" });
            var words1 = _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "mol" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "tost" });
            var words2 = _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "best" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "dron" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "step" });           
            _unitOfWork.Save();
            var countBefore = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == unlearnDic.Id).Count();
            //act      
            _unlearningWordsManager.RemoveUnlearningDictionary(nameNotLearn, words1).GetAwaiter().GetResult();
            //assert
            var countAfter = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == unlearnDic.Id).Count();
            Assert.AreEqual(countBefore-1, countAfter);
            //remove second
            //act      
            _unlearningWordsManager.RemoveUnlearningDictionary(nameNotLearn, words2).GetAwaiter().GetResult();
            //assert
            countAfter = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == unlearnDic.Id).Count();
            Assert.AreEqual(countBefore - 2, countAfter);
        }

       /// <summary>
       /// при продолжении изучении неизученных ранее слов. слова удаляются по мере их изучения
       /// </summary>
        [Test]
        public void RemoveUnlearningDictionary_RemoveFromUnlearningContinueDictionary()
        {
            var nameNotLearn = _defaultDictionary.Name + Resource.NotLearningPostfics;
            var nameContinueUnlearn = nameNotLearn + NAME_DB_FOR_CONTINUE;
            var lastId = _unitOfWork.DictionaryRepository.Get().LastOrDefault().Id;
            var contUnlDic = _unitOfWork.DictionaryRepository.Create(new Dictionary() { Id = ++lastId, Name = nameContinueUnlearn });
            var unlearnDic = _unitOfWork.DictionaryRepository.Create(new Dictionary() { Id = ++lastId, Name = nameNotLearn });
            _unitOfWork.Save();
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "fill" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "mol" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "tost" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "best" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "dron" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "step" });
            _unitOfWork.Save();
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = contUnlDic.Id, RusWord = "tost" });
            var words1 = _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = contUnlDic.Id, RusWord = "best" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = contUnlDic.Id, RusWord = "dron" });
            var words2 = _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = contUnlDic.Id, RusWord = "step" });
            _unitOfWork.Save();
            var countBefore = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == unlearnDic.Id).Count();
            //act      
            _unlearningWordsManager.RemoveUnlearningDictionary(nameNotLearn, words1).GetAwaiter().GetResult();
            //assert
            var countAfter = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == unlearnDic.Id).Count();
            Assert.AreEqual(countBefore - 1, countAfter);
            //remove second
            //act      
            _unlearningWordsManager.RemoveUnlearningDictionary(nameNotLearn, words2).GetAwaiter().GetResult();
            //assert
            countAfter = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == unlearnDic.Id).Count();
            Assert.AreEqual(countBefore - 2, countAfter);
        }



        /// <summary>
        /// при изучении всех ранее неизученных слов, они удаляются из словаря и словарь удаляется при количестве слов равных 0
        /// </summary>
        [Test]
        public void RemoveUnlearningDictionary_FullRemoveUnlearningDictionary()
        {
            var nameNotLearn = _defaultDictionary.Name + Resource.NotLearningPostfics;
            var lastId = _unitOfWork.DictionaryRepository.Get().LastOrDefault().Id;
            var unlearnDic = _unitOfWork.DictionaryRepository.Create(new Dictionary() { Id = ++lastId, Name = nameNotLearn });
            _unitOfWork.Save();
            var words1 = _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "fill" });
            var words2 = _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "mol" });
            var words3 = _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "tost" });
            var words4 = _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "best" });
            _unitOfWork.Save();
            var countBefore = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == unlearnDic.Id).Count();
            //act      
            _unlearningWordsManager.RemoveUnlearningDictionary(nameNotLearn, words1).GetAwaiter().GetResult();
            _unlearningWordsManager.RemoveUnlearningDictionary(nameNotLearn, words2).GetAwaiter().GetResult();
            _unlearningWordsManager.RemoveUnlearningDictionary(nameNotLearn, words3).GetAwaiter().GetResult();
            _unlearningWordsManager.RemoveUnlearningDictionary(nameNotLearn, words4).GetAwaiter().GetResult();
            //assert
            var countAfter = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == unlearnDic.Id)?.Count();
            Assert.AreEqual(0,countAfter);
            Assert.IsNull(_unitOfWork.DictionaryRepository.Get(unlearnDic.Id));
        }
    }
}
