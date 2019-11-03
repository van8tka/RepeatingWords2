using NUnit.Framework;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;


namespace RepeatingWords.NUnitTest
{
    [TestFixture]
    public class TestContinueWordsManager
    {
        internal const string NAME_DB_FOR_CONTINUE = "ContinueDictionary";
        private IUnitOfWork _unitOfWork;
        private IContinueWordsService _continue;
        private Dictionary _dictionary;
        private IEnumerable<Words> _words;

        [SetUp]
        public void Begin()
        {
            var container = UnityConfig.Load();          
            _unitOfWork = container.Resolve<IUnitOfWork>();
            var init = container.Resolve<IInitDefaultDb>();
            _continue = container.Resolve<IContinueWordsService>();
            init.LoadDefaultData();
            _dictionary = _unitOfWork.DictionaryRepository.Get().FirstOrDefault();
            _words = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == _dictionary.Id).AsEnumerable();
        }

        [TearDown]
        public void ResetDb()
        {
            var allDictionaries = _unitOfWork.DictionaryRepository.Get().AsEnumerable();
            foreach (var dictionary in allDictionaries)
            {
                if (!dictionary.Name.Equals(_dictionary.Name, StringComparison.OrdinalIgnoreCase))
                {
                    var words = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == dictionary.Id).AsEnumerable();
                    for (int i = 0; i < words.Count(); i++)
                        _unitOfWork.WordsRepository.Delete(words.ElementAt(i));
                    _unitOfWork.DictionaryRepository.Delete(dictionary);
                    _unitOfWork.Save();
                }
            }
        }



        [Test]
        public void SaveContinueDictionary_ReturnNewDictionary()
        {
            //act
            int iddictionary = _continue.SaveContinueDictionary(_dictionary.Name, _words.Skip(2), false);
            //assert
            var dictionaries = _unitOfWork.DictionaryRepository.Get();
            var dictionaryContinueName = _dictionary.Name + NAME_DB_FOR_CONTINUE;
            Assert.AreEqual(2, dictionaries.Count());
            Assert.IsTrue(_unitOfWork.DictionaryRepository.Get().Where(x => x.Name.Equals(dictionaryContinueName, StringComparison.OrdinalIgnoreCase)).Any());
            var wordsContinue = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == iddictionary);
            Assert.IsTrue(wordsContinue.Any());
            Assert.AreEqual(98, wordsContinue.Count());
        }


        [Test]
        public void SaveContinueDictionary_ReturnRecreateContinueDictionaryDictionary()
        {
            var dictionaryContinueName = _dictionary.Name + NAME_DB_FOR_CONTINUE;
            int iddictionaryNew = _continue.SaveContinueDictionary(_dictionary.Name, _words.Skip(2), false);
            var words = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == iddictionaryNew).AsEnumerable();
            Assert.AreEqual(98, words.Count());
            //act
            int iddictionaryContinue = _continue.SaveContinueDictionary(dictionaryContinueName, words.Skip(2).ToList(), false);
            //assert
            var dictionaries = _unitOfWork.DictionaryRepository.Get();        
            Assert.AreEqual(2, dictionaries.Count());
            Assert.AreEqual(iddictionaryNew, iddictionaryContinue);
            Assert.IsTrue(_unitOfWork.DictionaryRepository.Get().Where(x => x.Name.Equals(dictionaryContinueName, StringComparison.OrdinalIgnoreCase)).Any());
            var wordsContinue = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == iddictionaryContinue).AsEnumerable();
            Assert.IsTrue(wordsContinue.Any());
            Assert.AreEqual(96, wordsContinue.Count());
        }


        [Test]
        public void SaveContinueDictionary_ReturnUnlearningContinueDictionary()
        {
            var last = _unitOfWork.DictionaryRepository.Get().LastOrDefault();
            var unlearnDic = _unitOfWork.DictionaryRepository.Create(new Dictionary() { Id = last.Id+1, Name = _dictionary.Name + Resource.NotLearningPostfics });
            _unitOfWork.Save();
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "fill" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "mol" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "tost" });
           var w1 = _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "best" });
           var w2 =_unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "dron" });
           var w3 = _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "step" });
            _unitOfWork.Save();
            //act
            int iddictionaryNew = _continue.SaveContinueDictionary(_dictionary.Name+Resource.NotLearningPostfics, new List<Words>() {w1,w2,w3}, false);
            //assert
            var dictionaries = _unitOfWork.DictionaryRepository.Get();
            Assert.AreEqual(3, dictionaries.Count());        
            Assert.IsTrue(_unitOfWork.DictionaryRepository.Get().Where(x => x.Name.Equals(_dictionary.Name + Resource.NotLearningPostfics+ NAME_DB_FOR_CONTINUE, StringComparison.OrdinalIgnoreCase)).Any());
            var words = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == iddictionaryNew).AsEnumerable();
            Assert.AreEqual(3, words.Count());        
        }


        [Test]
        public void SaveContinueDictionary_ReturnContinueUnlearningContinueDictionary()
        {
            var last = _unitOfWork.DictionaryRepository.Get().LastOrDefault();
            var unlearnDic = _unitOfWork.DictionaryRepository.Create(new Dictionary() { Id = last.Id + 1, Name = _dictionary.Name + Resource.NotLearningPostfics });
            _unitOfWork.Save();
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "fill" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "mol" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "tost" });
            var w1 = _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "best" });
            var w2 = _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "dron" });
            var w3 = _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = unlearnDic.Id, RusWord = "step" });
            _unitOfWork.Save();
            var nameUnlearnimg = _dictionary.Name + Resource.NotLearningPostfics;
            var nameUnlContinye = _dictionary.Name + Resource.NotLearningPostfics + NAME_DB_FOR_CONTINUE;
            int iddictionaryNew = _continue.SaveContinueDictionary(nameUnlearnimg, new List<Words>() { w1, w2, w3 }, false);         
            var dictionaries = _unitOfWork.DictionaryRepository.Get();
            Assert.AreEqual(3, dictionaries.Count());
            Assert.IsTrue(_unitOfWork.DictionaryRepository.Get().Where(x => x.Name.Equals(nameUnlContinye, StringComparison.OrdinalIgnoreCase)).Any());
            var words = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == iddictionaryNew).ToList();
            Assert.AreEqual(3, words.Count());
            //act
            iddictionaryNew = _continue.SaveContinueDictionary(nameUnlContinye, words.Skip(1), false);
            //assert
            dictionaries = _unitOfWork.DictionaryRepository.Get();
            Assert.AreEqual(3, dictionaries.Count());
            words = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == iddictionaryNew).ToList();
            Assert.AreEqual(2, words.Count());
        }






        [Test]
        public void RemoveContinueDictionary_ReturnTrue()
        {        
            
            int idDictionary = _continue.SaveContinueDictionary(_dictionary.Name, _words.Skip(2), false);
            var words = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == idDictionary).AsEnumerable();
            //act
            bool success = _continue.RemoveContinueDictionary(words);
            //assert
            var dictionaries = _unitOfWork.DictionaryRepository.Get();
            var last = _unitOfWork.LastActionRepository.Get().LastOrDefault();
            Assert.AreEqual(1, dictionaries.Count());          
            Assert.IsNull(last);                
        }
    }
}
