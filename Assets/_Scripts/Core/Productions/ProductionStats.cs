using System;
using System.Numerics;

namespace _Scripts.Core.Productions
{
    public struct ProductionStats
    {
        public bool AutoProduction;
        public BigInteger ProductionCount;
        public float ProductionRate;

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

    [Serializable]
    public struct SerializableProductionStats
    {
        public bool autoProduction;
        public long productionCount;
        public float productionRate;

        public ProductionStats ToProductionStats()
        {
            return new ProductionStats(productionCount, productionRate, autoProduction);
        }
    }
}