using NUnit.Framework;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using System.Linq;
using Unity;

namespace RepeatingWords.NUnitTest
{
    [TestFixture]
  public class TestLastActionRepository
    {
        private IUnitOfWork _unitOfWork;
        [SetUp]
        public void Setup()
        {
            var container = UnityConfig.Load();
            _unitOfWork = container.Resolve<IUnitOfWork>();
        }

        [Test]
        public void Test_Create_CreateLastActionInDb_ReturnLastAction()
        {
            // arrange          
            var lastAct = new LastAction() { Id = 0, IdDictionary = 1, IdWord=2, FromRus = true };
            // act
            var result = _unitOfWork.LastActionRepository.Create(lastAct);
            _unitOfWork.Save();
            // assert
            Assert.IsInstanceOf(typeof(LastAction), result);
            Assert.AreNotEqual(0, result.Id);
            Assert.AreEqual(1, result.IdDictionary);
            Assert.AreEqual(2, result.IdWord);
            Assert.IsTrue( result.FromRus);
        }

        [Test]
        public void Test_Get_GetLastActionFromDb_Return2LastActions()
        {
            //arrange          
            _unitOfWork.LastActionRepository.Create(new LastAction { Id = 0, IdDictionary = 1, IdWord = 2, FromRus = true });
            _unitOfWork.LastActionRepository.Create(new LastAction { Id = 0, IdDictionary = 2, IdWord = 3, FromRus = false });
            _unitOfWork.Save();
            //act
            var result = _unitOfWork.LastActionRepository.Get().ToList();
            //assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void Test_GetByID_GetLastActionFromDb_ReturnConcreteLastAction()
        {
            //arrange          
            _unitOfWork.LastActionRepository.Create(new LastAction { Id = 0, IdDictionary = 1, IdWord = 2, FromRus = true });
            var lastAct = _unitOfWork.LastActionRepository.Create(new LastAction { Id = 0, IdDictionary = 2, IdWord = 3, FromRus = false });
            _unitOfWork.Save();
            //act
            var result = _unitOfWork.LastActionRepository.Get(lastAct.Id);
            //assert
            Assert.IsInstanceOf(typeof(LastAction), result);
            Assert.AreEqual(lastAct.Id, result.Id);
            Assert.IsFalse(result.FromRus);
        }

        [Test]
        public void Test_Update_UpdateLastActionInDb_ReturnUpdatedLastAction()
        {
            //arrange
            var fromRus = true;
            var updateFromRus = false;
            var lastActBeforeUpd = _unitOfWork.LastActionRepository.Create(new LastAction { Id = 0, IdDictionary = 1, IdWord = 2, FromRus = fromRus });
            _unitOfWork.Save();
            //act
            lastActBeforeUpd.FromRus = updateFromRus;
            _unitOfWork.LastActionRepository.Update(lastActBeforeUpd);
            _unitOfWork.Save();
            var lastActAfterUpdate = _unitOfWork.LastActionRepository.Get(lastActBeforeUpd.Id);
            //assert
            Assert.AreEqual(updateFromRus, lastActBeforeUpd.FromRus);
        }

        [Test]
        public void Test_Delete_DeleteLastActFromDb_ReturnTrue()
        {
            //arrange                    
            var lastAct = _unitOfWork.LastActionRepository.Create(new LastAction { Id = 0, IdDictionary = 1, IdWord = 2, FromRus = true  });
            _unitOfWork.Save();
            var countBeforeRemove = _unitOfWork.LastActionRepository.Get().ToList().Count();
            //act
            var result = _unitOfWork.LastActionRepository.Delete(lastAct);
            _unitOfWork.Save();
            var countAfterRemove = _unitOfWork.LastActionRepository.Get().ToList().Count();
            //assert
            Assert.IsTrue(result);
            Assert.Less(countAfterRemove, countBeforeRemove);
            Assert.AreEqual(0, countAfterRemove);
        }
    }
}
