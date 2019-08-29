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
        private IUnlearningWordsManager _unlearningWordsManager;
        private IContinueWordsManager _continueWordsManager;
        private IUnitOfWork _unitOfWork;
        private Dictionary _defaultDictionary;
        private IEnumerable<Words> _wordsAllDefaultCollection;
        internal const string NAME_DB_FOR_CONTINUE = "ContinueDictionary";

        public TestUnlearningManager()
        {
            var container = UnityConfig.Load();           
            _unitOfWork = container.Resolve<IUnitOfWork>();
            _continueWordsManager = container.Resolve<IContinueWordsManager>();
            _unlearningWordsManager = container.Resolve<IUnlearningWordsManager>();          
            var init = container.Resolve<IInitDefaultDb>();
            init.LoadDefaultData();
            _defaultDictionary = _unitOfWork.DictionaryRepository.Get().FirstOrDefault();
            _wordsAllDefaultCollection = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == _defaultDictionary.Id).AsEnumerable();
        }



        [SetUp]
        public void Setup()
        {

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


       
        [Test]
        public void Test_SaveUnlearningDictionary_UnExistUnlearnedDictionary_IdDictionary()
        {
            int countUnlearnedWords = 2;
            var wordsUnlearn = _wordsAllDefaultCollection.Take(countUnlearnedWords);
            //act
            int id = _unlearningWordsManager.SaveUnlearningDictionary(_defaultDictionary.Name, wordsUnlearn, null,null);
            //assert
            Assert.Greater(id, 0);
            Assert.AreEqual(_defaultDictionary.Name + Resource.NotLearningPostfics, _unitOfWork.DictionaryRepository.Get(id).Name);
            Assert.AreEqual(countUnlearnedWords, _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == id).Count());           
            CollectionAssert.AreEqual(wordsUnlearn.Select(x=>x.RusWord).OrderByDescending(x=>x), _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == id).Select(x=>x.RusWord).AsEnumerable().OrderByDescending(x => x));
        }

        [Test]
        public void Test_SaveUnlearningDictionary_Continue_IdDictionary()
        {
            int countUnlearnedWords = 2;
            var wordsUnlearn1 = _wordsAllDefaultCollection.Take(countUnlearnedWords);
            var wordsUnlearn2 = _wordsAllDefaultCollection.Skip(countUnlearnedWords).Take(countUnlearnedWords);
            var wordsLeft = _wordsAllDefaultCollection.Skip(4).Take(countUnlearnedWords);
            var wordsAll = _wordsAllDefaultCollection.Skip(2);
            var nameContinueDictionary = _defaultDictionary.Name + NAME_DB_FOR_CONTINUE;
            //act
            _continueWordsManager.SaveContinueDictionary(nameContinueDictionary, wordsAll.ToList(), true);
            _unlearningWordsManager.SaveUnlearningDictionary(_defaultDictionary.Name, wordsUnlearn1, null, null);
            int id = _unlearningWordsManager.SaveUnlearningDictionary(nameContinueDictionary, wordsUnlearn2, wordsLeft, wordsAll);
            //assert
            Assert.Greater(id, 0);
            Assert.AreEqual(_defaultDictionary.Name + Resource.NotLearningPostfics, _unitOfWork.DictionaryRepository.Get(id).Name);
            Assert.AreEqual(3, _unitOfWork.DictionaryRepository.Get().Count());
            Assert.AreEqual(4, _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == id).Count());
            CollectionAssert.AreEqual(wordsUnlearn1.Union(wordsUnlearn2).Select(x => x.RusWord).OrderByDescending(x => x), _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == id).Select(x => x.RusWord).AsEnumerable().OrderByDescending(x => x));
        }


        [Test]
        public void Test_SaveUnlearningDictionary_Unlearn_IdDictionary()
        {
            int countUnlearnedWords = 2;
            var wordsUnlearn1 = _wordsAllDefaultCollection.Take(countUnlearnedWords);
            var wordsUnlearn2 = _wordsAllDefaultCollection.Skip(countUnlearnedWords).Take(countUnlearnedWords);
            var wordsLeft = _wordsAllDefaultCollection.Skip(4).Take(countUnlearnedWords);
            var wordsAll = _wordsAllDefaultCollection.Skip(2);
           
            var nameContinueDictionary = _defaultDictionary.Name + NAME_DB_FOR_CONTINUE;
            var nameNotLearn = _defaultDictionary.Name + Resource.NotLearningPostfics;
           
            _continueWordsManager.SaveContinueDictionary(nameContinueDictionary, wordsAll.ToList(), true);
            _unlearningWordsManager.SaveUnlearningDictionary(_defaultDictionary.Name, wordsUnlearn1, null, null);
            int id = _unlearningWordsManager.SaveUnlearningDictionary(nameContinueDictionary, wordsUnlearn2, wordsLeft, wordsAll);
            var wordsUnlearnAll3 = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == id).ToList();
            wordsLeft = wordsUnlearnAll3.Skip(2); 
            var wordsUnlearn3 = wordsUnlearnAll3.Take(1); 
            //act      
            id = _unlearningWordsManager.SaveUnlearningDictionary(nameNotLearn, wordsUnlearn3, wordsLeft, wordsUnlearnAll3);        
            //assert
            Assert.Greater(id, 0);
            Assert.AreEqual(nameNotLearn, _unitOfWork.DictionaryRepository.Get(id).Name);
            Assert.AreEqual(3, _unitOfWork.DictionaryRepository.Get().Count());
            Assert.AreEqual(3, _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == id).Count());
             wordsUnlearnAll3.RemoveAt(1);             
            CollectionAssert.AreEqual(wordsUnlearnAll3.Select(x=>x.RusWord).OrderByDescending(x => x), _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == id).Select(x => x.RusWord).AsEnumerable().OrderByDescending(x => x));
        }



        [Test]
        public void Test_SaveUnlearningDictionary_UnlearnContinue_IdDictionary()
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
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = contUnlDic.Id, RusWord = "best" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = contUnlDic.Id, RusWord = "dron" });
            _unitOfWork.WordsRepository.Create(new Words() { Id = 0, IdDictionary = contUnlDic.Id, RusWord = "step" });
            _unitOfWork.Save();
            var all = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == contUnlDic.Id).ToList();
            var allUnlearn = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == unlearnDic.Id).AsEnumerable();
            var unlWords = all.Skip(1).Take(2);
            var left = all.Skip(3);
            //act
            var id = _unlearningWordsManager.SaveUnlearningDictionary(nameContinueUnlearn, unlWords, left, all);
            //assert
            Assert.Greater(id, 0);
            Assert.AreEqual(nameNotLearn, _unitOfWork.DictionaryRepository.Get(id).Name);
            Assert.AreEqual(3, _unitOfWork.DictionaryRepository.Get().Count());
            Assert.AreEqual(5, _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == id).Count());     
            var listExpect = allUnlearn.Select(x => x.RusWord).ToList();
            var listActual = _unitOfWork.WordsRepository.Get().Where(x => x.IdDictionary == id).Select(x => x.RusWord).ToList();
            CollectionAssert.AreEqual(listExpect, listActual);
        }
    }
}
