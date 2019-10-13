using Unity;
using NUnit.Framework;
using RepeatingWords.Interfaces;

namespace RepeatingWords.NUnitTest
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
        public void Test_LoadDefaultData_InvokeInitDefData_InitIsTrue()
        {
          var result = _init.LoadDefaultData();
          Assert.IsTrue(result);
        }
    }
}