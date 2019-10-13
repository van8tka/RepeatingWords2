using NUnit.Framework;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using System.Linq;
using Unity;

namespace RepeatingWords.NUnitTest
{
    [TestFixture]
   public class TestDictionaryRepository
    {
        private IUnitOfWork _unitOfWork;
        [SetUp]
        public void Setup()
        {
            var container =  UnityConfig.Load();
            _unitOfWork = container.Resolve<IUnitOfWork>();
        }

        [Test]
        public void Test_Create_CreateDictionaryInDb_ReturnDictionary()
        {
            // arrange
            var name = "testnamedictionary";
            var dic = new Dictionary() { Id = 0, Name = name};
            // act
            var result = _unitOfWork.DictionaryRepository.Create(dic);
            _unitOfWork.Save();
            // assert
            Assert.IsInstanceOf(typeof(Dictionary), result);
            Assert.AreNotEqual(0, result.Id);
            Assert.AreEqual(name, result.Name);
        }

        [Test]
        public void Test_Get_GetDictionaryFromDb_Return2Dictionary()
        {
            //arrange 
            var nameDic1 = "testDicName1";
            var nameDic2 = "testDicName2";
            _unitOfWork.DictionaryRepository.Create(new Dictionary { Id = 0, Name = nameDic1 });
            _unitOfWork.DictionaryRepository.Create(new Dictionary { Id = 0, Name = nameDic2 });
            _unitOfWork.Save();
            //act
            var result = _unitOfWork.DictionaryRepository.Get().ToList();
            //assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void Test_GetByID_GetDictionaryFromDb_ReturnConcreteDictionary()
        {
            //arrange 
            var nameDic1 = "testDicName1";
            var nameDic2 = "testDicName2";
            _unitOfWork.DictionaryRepository.Create(new Dictionary { Id = 0, Name = nameDic1 });
            var dic = _unitOfWork.DictionaryRepository.Create(new Dictionary { Id = 0, Name = nameDic2 });
            _unitOfWork.Save();
            //act
            var result = _unitOfWork.DictionaryRepository.Get(dic.Id);
            //assert
            Assert.IsInstanceOf(typeof(Dictionary), result);
            Assert.AreEqual(dic.Id, result.Id);
            Assert.AreEqual(nameDic2, result.Name);
        }

        [Test]
        public void Test_Update_UpdateDictionaryInDb_ReturnUpdatedDictionary()
        {
            //arrange
            var name = "testName";
            var updateName = "UpdatedName";
            var dictionaryBeforeUpd = _unitOfWork.DictionaryRepository.Create(new Dictionary { Id = 0, Name = name });
            _unitOfWork.Save();
            //act
            dictionaryBeforeUpd.Name = updateName;
            _unitOfWork.DictionaryRepository.Update(dictionaryBeforeUpd);
            _unitOfWork.Save();
            var dictionaryAfterUpdate = _unitOfWork.DictionaryRepository.Get(dictionaryBeforeUpd.Id);
            //assert
            Assert.AreEqual(updateName, dictionaryAfterUpdate.Name);
        }

        [Test]
        public void Test_Delete_DeleteDictionaryFromDb_ReturnTrue()
        {
            //arrange                    
            var dictionary = _unitOfWork.DictionaryRepository.Create(new Dictionary { Id = 0, Name = "testName" });
            _unitOfWork.Save();
            var countBeforeRemove = _unitOfWork.DictionaryRepository.Get().ToList().Count();
            //act
            var result = _unitOfWork.DictionaryRepository.Delete(dictionary);
            _unitOfWork.Save();
            var countAfterRemove = _unitOfWork.DictionaryRepository.Get().ToList().Count();
            Assert.IsTrue(result);
            Assert.Less(countAfterRemove, countBeforeRemove);
            Assert.AreEqual(0, countAfterRemove);
        }
    }
}
