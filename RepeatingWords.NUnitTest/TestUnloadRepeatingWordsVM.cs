using NUnit.Framework;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Interfaces;
using RepeatingWords.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace RepeatingWords.NUnitTest
{
    [TestFixture]
    public class TestUnloadRepeatingWordsVM
    {
        private  RepeatingWordsViewModel _vm;
        private IUnitOfWork _unitOfWork;

        [SetUp]
        public void Begin()
        {
            var container = UnityConfig.Load();
            _vm = container.Resolve<RepeatingWordsViewModel>();
              _unitOfWork = container.Resolve<IUnitOfWork>();
            var init = container.Resolve<IInitDefaultDb>();
            init.LoadDefaultData();
            _vm.InitializeAsync(_unitOfWork.DictionaryRepository.Get().FirstOrDefault()).GetAwaiter().GetResult();
        }


        [Test]
        public void TestSetSelectingWords()
        {
            _vm.Model.wordsCollectionLeft.Add(_vm.Model.wordsCollection.ElementAt(0));
            _vm.Model.wordsCollectionLeft.Add(_vm.Model.wordsCollection.ElementAt(1));

            _vm.Model.wordsOpen.Add(_vm.Model.wordsCollection.ElementAt(0));
            _vm.Model.wordsOpen.Add(_vm.Model.wordsCollection.ElementAt(1));
         
            _vm.Model.AllShowedWordsCount = 2;
            _vm.Model.AllOpenedWordsCount = 2;

            _vm.UnloadPage();

            var dictionaries = _unitOfWork.DictionaryRepository.Get().ToList();
            var last = _unitOfWork.LastActionRepository.Get().ToList();
            var words = _unitOfWork.WordsRepository.Get().ToList();

            _vm.Model.wordsCollectionLeft = new List<Words>();
            _vm.Model.wordsOpen = new List<Words>();
            _vm.InitializeAsync(_unitOfWork.DictionaryRepository.Get().ToList().ElementAt(1)).GetAwaiter().GetResult();
            _vm.Model.wordsCollectionLeft.Add(_vm.Model.wordsCollection.ElementAt(0));
            _vm.Model.wordsOpen.Add(_vm.Model.wordsCollection.ElementAt(0));
            _vm.Model.AllShowedWordsCount = 1;
            _vm.Model.AllOpenedWordsCount = 1;
            _vm.UnloadPage();
             dictionaries = _unitOfWork.DictionaryRepository.Get().ToList();
             last = _unitOfWork.LastActionRepository.Get().ToList();
             words = _unitOfWork.WordsRepository.Get().ToList();
        }
    }
}
