using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Internal;
using RepeatingWords.Helpers;
using RepeatingWords.Helpers.Interfaces;

namespace RepeatingWords.NUnitTest
{
    [TestFixture]
    public class TestEntryWordValidator
    {
        private IEntryWordValidator _wordValidator;
        public TestEntryWordValidator()
        {
            _wordValidator = new EntryWordValidator();
        }

        [TestCase( @"варенье(тест бол)", "тест бол", true)]
        [TestCase( @" (ДОм), (тест), (Тест), (ЗДЕСЬ)", "тест", true)]
        [TestCase( @" (ДОм) (тест) (Тест) (ЗДЕСЬ)", "тест", true)]
        [TestCase(@" ДОм / тест / Тест / ЗДЕСЬ", "тест", true)]
        [TestCase( @" ЗДЕСЬ\тест \ Тест \ ДОм", "тест", true)]
        [TestCase( "тест, ТЕСТ, ЗДЕСЬ, ДОм", "тест", true)]
        [TestCase( @"тест где-то", "тест где-то", true)]
        [TestCase("    , табуляция,     пробел", " пробел", true)]
        [TestCase("    , табуляция,     пробел", " варенье, крем", false)]
        [TestCase("тест бол", @"варенье(тест бол)", true)]
        [TestCase("тест", @" (ДОм), (тест), (Тест), (ЗДЕСЬ)", true)]
        [TestCase("тест", @" (ДОм) (тест) (Тест) (ЗДЕСЬ)", true)]
        [TestCase("тест", @" ДОм / тест / Тест / ЗДЕСЬ", true)]
        [TestCase("тест", @" ЗДЕСЬ\тест \ Тест \ ДОм", true)]
        [TestCase("тест", "тест, ТЕСТ, ЗДЕСЬ, ДОм", true)]
        [TestCase("тест где-то", @"тест где-то", true)]
        [TestCase("тест","ТЕСТ",true)]
        [TestCase("тест", "notValid",false)]
        [TestCase("тест", "тест", true)]
        [TestCase(" пробел", "   пробел", true)]
        [TestCase(" пробел", "    пробел, табуляция", true)]
        [Test]
        public void TestWordValidator(string entry, string original, bool resultAssert)
        {
            var result = _wordValidator.IsValidWord(entry, original);
            Assert.IsTrue(result == resultAssert);
        }

    }
}
