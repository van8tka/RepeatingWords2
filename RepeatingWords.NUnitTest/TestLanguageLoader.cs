using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Internal;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using Unity;

namespace RepeatingWords.NUnitTest
{
    [TestFixture]
   public class TestLanguageLoader
    {
        public TestLanguageLoader()
        { }

        private IUnitOfWork _unitOfWork;
        private ILanguageLoaderFacade _languageLoader;
        [SetUp]
        public void Setup()
        {
            var container = UnityConfig.Load();
            _unitOfWork = container.Resolve<IUnitOfWork>();
            _languageLoader = container.Resolve<ILanguageLoaderFacade>();
        }

        [Test]
        public void Test_ParseJArrayFromString()
        {
            _unitOfWork.DictionaryRepository.Create(new Dictionary() { Id = 1, Name = "Test" });
            _unitOfWork.Save();
            //fixme: change id language
            Assert.True(false);
            _languageLoader.LoadSelectedLanguageToDB(6,"Test").GetAwaiter().GetResult();
            var dict = _unitOfWork.DictionaryRepository.Get().ToList();
            var words = _unitOfWork.WordsRepository.Get().Where(x=>x.IdDictionary == 6).ToList();
            Assert.IsNotNull(dict);
            Assert.Greater(dict.Count(), 1);
            Assert.IsNotNull(words);
            Assert.Greater(words.Count(), 0);
        }
    }
}
