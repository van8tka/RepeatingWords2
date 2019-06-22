using NUnit.Framework;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using System.Linq;
using Unity;

namespace RepeatingWords.NUnitTest
{
    [TestFixture]
   public class TestWordsRepository
    {
        private IUnitOfWork _unitOfWork;
        [SetUp]
        public void Setup()
        {
            var container = UnityConfig.Load();
            _unitOfWork = container.Resolve<IUnitOfWork>();
        }


        [Test]
        public void Test_Create_CreateWordInDb_ReturnWord()
        {
            // arrange
            var rus = "слово";
            var eng = "word";
            var transcript = "[wərd]";
            var word = new Words() { Id = 0,IdDictionary = 1,RusWord = rus, EngWord = eng, Transcription = transcript};
            // act
            var result = _unitOfWork.WordsRepository.Create(word);
            _unitOfWork.Save();
            // assert
            Assert.IsInstanceOf(typeof(Words), result);
            Assert.AreNotEqual(0, result.Id);
            Assert.AreEqual(rus, result.RusWord);
            Assert.AreEqual(eng, result.EngWord);
            Assert.AreEqual(transcript, result.Transcription);
        }

        [Test]
        public void Test_Get_GetWordFromDb_Return2Words()
        {
            //arrange          
            _unitOfWork.WordsRepository.Create(new Words { Id = 0,IdDictionary=1,RusWord="тест",EngWord="test",Transcription="[test]"});
            _unitOfWork.WordsRepository.Create(new Words { Id = 0, IdDictionary = 1, RusWord = "тест2", EngWord = "test2", Transcription = "[test2]" });
            _unitOfWork.Save();
            //act
            var result = _unitOfWork.WordsRepository.Get().ToList();
            //assert
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void Test_GetByID_GetWordFromDb_ReturnConcreteWord()
        {
            //arrange
            var rus = "тест2";
            _unitOfWork.WordsRepository.Create(new Words { Id = 0, IdDictionary = 1, RusWord = "тест2", EngWord = "test", Transcription = "[test]" });
            var word = _unitOfWork.WordsRepository.Create(new Words { Id = 0, IdDictionary = 1, RusWord = rus, EngWord = "test2", Transcription = "[test2]" });
            _unitOfWork.Save();         
            //act
            var result = _unitOfWork.WordsRepository.Get(word.Id);
            //assert
            Assert.IsInstanceOf(typeof(Words), result);
            Assert.AreEqual(word.Id, result.Id);
            Assert.AreEqual(rus, result.RusWord);
        }

        [Test]
        public void Test_Update_UpdateWordInDb_ReturnUpdatedWord()
        {
            //arrange
            var eng = "test";
            var updateEng = "UpdatedTest";
            var wordBeforeUpd = _unitOfWork.WordsRepository.Create(new Words { Id = 0, IdDictionary=1, EngWord=eng, RusWord="тест", Transcription="[test]" });
            _unitOfWork.Save();
            //act
            wordBeforeUpd.EngWord = updateEng;
            _unitOfWork.WordsRepository.Update(wordBeforeUpd);
            _unitOfWork.Save();
            var wordAfterUpdate = _unitOfWork.WordsRepository.Get(wordBeforeUpd.Id);
            //assert
            Assert.AreEqual(updateEng, wordAfterUpdate.EngWord);
        }

        [Test]
        public void Test_Delete_DeleteWordFromDb_ReturnTrue()
        {
            //arrange                    
            var word = _unitOfWork.WordsRepository.Create(new Words { Id = 0, IdDictionary = 1, EngWord="test", RusWord="тест", Transcription="[test]"});
            _unitOfWork.Save();
            var countBeforeRemove = _unitOfWork.WordsRepository.Get().ToList().Count();
            //act
            var result = _unitOfWork.WordsRepository.Delete(word);
            _unitOfWork.Save();
            var countAfterRemove = _unitOfWork.WordsRepository.Get().ToList().Count();
            //assert
            Assert.IsTrue(result);
            Assert.Less(countAfterRemove, countBeforeRemove);
            Assert.AreEqual(0, countAfterRemove);
        }

    }
}
