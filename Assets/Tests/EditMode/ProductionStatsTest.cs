using System.Numerics;
using _Scripts;
using NUnit.Framework;

namespace Tests.EditMode
{
    public class ProductionStatsTest
    {
        [Test]
        public void ProductionStats_GetProductionSpeedTest()
        {
            var productionStats = new ProductionStats(true, 100, 1.5f);
            Assert.AreEqual((BigInteger)66, productionStats.GetProductionSpeed());
        }
        
        [Test]
        public void ProductionStats_GetProductionSpeedTest2()
        {
            var productionStats = new ProductionStats(true, 128, 0.5f);
            Assert.AreEqual((BigInteger)256, productionStats.GetProductionSpeed());
        }
    }
}
