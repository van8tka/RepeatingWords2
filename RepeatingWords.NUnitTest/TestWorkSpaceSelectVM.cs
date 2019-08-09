using NUnit.Framework;
using RepeatingWords.DataService.Model;
using RepeatingWords.Model;
using RepeatingWords.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepeatingWords.NUnitTest
{
    [TestFixture]
    public class TestWorkSpaceSelectVM
    {

        private WorkSpaceSelectWordViewModel vm;
        private RepeatingWordsModel model;
        private Words word;

        [SetUp]
        public void Begin()
        {
            vm = new WorkSpaceSelectWordViewModel();
            model = new RepeatingWordsModel();
            model.wordsCollection = new List<Words>()
            {
                new Words{Id=0,IdDictionary=0,EngWord="0",RusWord="0-",Transcription="-0-"},
                 new Words{Id=1,IdDictionary=1,EngWord="1",RusWord="1-",Transcription="-1-"},
                  new Words{Id=2,IdDictionary=2,EngWord="2",RusWord="2-",Transcription="-2-"},
                   new Words{Id=3,IdDictionary=3,EngWord="3",RusWord="3-",Transcription="-3-"},
                    new Words{Id=4,IdDictionary=4,EngWord="4",RusWord="4-",Transcription="-4-"}
            };
            vm.Model = model;
            word = model.wordsCollection.First();
        }


        [Test]
        public void TestSetSelectingWords( )
        {
            vm.SetSelectingWords(word, true);
        }
    }
}
