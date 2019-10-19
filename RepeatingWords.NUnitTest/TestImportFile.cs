using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using NUnit.Framework;
using RepeatingWords.DataService.Interfaces;
using RepeatingWords.DataService.Model;
using RepeatingWords.Helpers.Interfaces;
using RepeatingWords.Interfaces;
using Unity;

namespace RepeatingWords.NUnitTest
{
    [TestFixture]
   public class TestImportFile
    {
        [SetUp]
        public void Begin()
        {
            var container = UnityConfig.Load();
            _unitOfWork = container.Resolve<IUnitOfWork>();
            _importFile = container.Resolve<IImportFile>();
        }


        [OneTimeSetUp]
        public void ClassInit()
        {
            _inputGood = Path.Combine(TestContext.CurrentContext.TestDirectory, "..//..//..//TestFiles//_inputGood.txt");
          _inputEmpty = Path.Combine(TestContext.CurrentContext.TestDirectory, "..//..//..//TestFiles//_inputEmpty.txt");
            _inputFirstLineEmpty = Path.Combine(TestContext.CurrentContext.TestDirectory, "..//..//..//TestFiles//_inputFirstLineEmpty.txt");
            _inputOtherFormateFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "..//..//..//TestFiles//_inputOtherFormateFile.ppt");
            _inputBadSecondLine = Path.Combine(TestContext.CurrentContext.TestDirectory, "..//..//..//TestFiles//_inputBadSecondLine.txt");
            _inputBadFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "..//..//..//TestFiles//_inputBadFile.txt");
            _inputBadDataInFile = Path.Combine(TestContext.CurrentContext.TestDirectory, "..//..//..//TestFiles//_inputBadDataInFile.txt");
        }

        


        private static string _inputGood;
        private string _inputEmpty;
        private string _inputFirstLineEmpty;
        private string _inputOtherFormateFile;
        private string _inputBadSecondLine;
        private string _inputBadFile;
        private string _inputBadDataInFile;

        private IImportFile _importFile;
        private IUnitOfWork _unitOfWork;

        [Test]
        public void Test_StartImport_GoodFile_True()
        {
            string fileName = Path.GetFileName(_inputGood);
            byte[] bytes = GetBytesFile(_inputGood);
            int idDictionary = CreateDictionary();
            var result = _importFile.StartImport(bytes, fileName, idDictionary).Result;
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_StartImport_EmptyFile_False()
        {
            string fileName = Path.GetFileName(_inputEmpty);
            byte[] bytes = GetBytesFile(_inputEmpty);
            int idDictionary = CreateDictionary();
            var result = _importFile.StartImport(bytes, fileName, idDictionary).Result;
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_StartImport_FirstLineEmpty_False()
        {
            string fileName = Path.GetFileName(_inputFirstLineEmpty);
            byte[] bytes = GetBytesFile(_inputFirstLineEmpty);
            int idDictionary = CreateDictionary();
            var result = _importFile.StartImport(bytes, fileName, idDictionary).Result;
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_StartImport_OtherFormateFile_False()
        {
            string fileName = Path.GetFileName(_inputOtherFormateFile);
            byte[] bytes = GetBytesFile(_inputOtherFormateFile);
            int idDictionary = CreateDictionary();
            var result = _importFile.StartImport(bytes, fileName, idDictionary).Result;
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_StartImport_BadSecondLine_False()
        {
            string fileName = Path.GetFileName(_inputBadSecondLine);
            byte[] bytes = GetBytesFile(_inputBadSecondLine);
            int idDictionary = CreateDictionary();
            var result = _importFile.StartImport(bytes, fileName, idDictionary).Result;
            Assert.IsFalse(result);
        }

        [Test]
        public void Test_StartImport_BadFile_False()
        {
            string fileName = Path.GetFileName(_inputBadFile);
            byte[] bytes = GetBytesFile(_inputBadFile);
            int idDictionary = CreateDictionary();
            var result = _importFile.StartImport(bytes, fileName, idDictionary).Result;
            Assert.IsFalse(result);
        }
        [Test]
        public void Test_StartImport_BadDataInFile_False()
        {
            string fileName = Path.GetFileName(_inputBadDataInFile);
            byte[] bytes = GetBytesFile(_inputBadDataInFile);
            int idDictionary = CreateDictionary();
            var result = _importFile.StartImport(bytes, fileName, idDictionary).Result;
            Assert.IsFalse(result);
        }






        private int CreateDictionary()
        {
            try
            {
                var dictionary = _unitOfWork.DictionaryRepository.Create(new Dictionary {Id = 0, Name = "Dictionary0"});
                _unitOfWork.Save();
                return dictionary.Id;
            }
            catch (Exception e)
            {
                Debugger.Break();
                throw;
            }
        }

        private byte[] GetBytesFile(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }
    }
}
