using NUnit.Framework;

namespace Tests.EditMode
{
    public class OfflineIncomeManagerTests
    {
        private int _i;
        [Test]
        public void CalculateOfflineIncomeTest()
        {
            Assert.AreEqual(1234, _i);
        }

        [SetUp]
        public void Setup()
        {
            _i = 1234;
        }
    }
}