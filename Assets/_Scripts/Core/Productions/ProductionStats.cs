using System.Numerics;

namespace _Scripts.Core.Productions
{
    public struct ProductionStats
    {
        public bool AutoProduction { get; private set; }
        public BigInteger ProductionCount { get; private set; }
        public float ProductionRate { get; private set; }
        
        public ProductionStats(BigInteger productionCount, float productionRate, bool autoProduction = false)
        {
            AutoProduction = autoProduction;
            ProductionCount = productionCount;
            ProductionRate = productionRate;
        }

        public override string ToString()
        {
            return
                $"{nameof(AutoProduction)}: {AutoProduction}, {nameof(ProductionCount)}: {ProductionCount}, {nameof(ProductionRate)}: {ProductionRate}";
        }
    }
}