using Unity;
using NUnit.Framework;
using RepeatingWords.Interfaces;
using RepeatingWords.NUnitTest;

namespace Tests
{
    public class TestInitDbDefault
    {

        private IInitDefaultDb _init;
        [SetUp]
        public void Setup()
        {
            var container = UnityConfig.Load();
            _init = container.Resolve<IInitDefaultDb>();
        }

        [Test]
        public void TestCreateDb()
        {
            _init.LoadDefaultData();
            Assert.Pass();
        }
    }
}