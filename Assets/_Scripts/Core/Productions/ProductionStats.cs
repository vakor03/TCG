using System;
using System.Collections.Generic;
using System.Numerics;
using _Scripts.Core.Upgrades;
using _Scripts.Helpers;
using _Scripts.ScriptableObjects;

namespace _Scripts.Core.Productions
{
    public class ProductionStats
    {
        public bool AutoProduction { get; private set; }
        public BigInteger ProductionCount { get; private set; }
        public float ProductionRate { get; private set; }
        private List<ProductionUpgrade> _productionUpgrades;

        public event Action<bool> OnAutoProductionChanged;
        public event Action OnProductionCountChanged;
        public event Action OnProductionRateChanged;

        public ProductionStats(ProductionSO productionSO)
        {
            AutoProduction = false;
            ProductionCount = productionSO.ProductionCount;
            ProductionRate = productionSO.BaseProductionRate;
        }

        public ProductionStats(bool autoProduction, BigInteger productionCount, float productionRate)
        {
            AutoProduction = autoProduction;
            ProductionCount = productionCount;
            ProductionRate = productionRate;
        }

        public BigInteger GetProductionSpeed()
        {
            return ProductionCount.Divide(ProductionRate, 3);
        }

        public void UnlockUpgrade(ProductionUpgrade productionUpgrade)
        {
            if (productionUpgrade.UpgradeType == UpgradeType.AutoProduction)
            {
                AutoProduction = true;
                OnAutoProductionChanged?.Invoke(true);
            }
            else if (productionUpgrade.UpgradeType == UpgradeType.ProductionRate)
            {
                var productionRateUpgrade = (ProductionRateUpgrade) productionUpgrade;
                ProductionRate /= productionRateUpgrade.Value;
                OnProductionRateChanged?.Invoke();
            }
            else if (productionUpgrade.UpgradeType == UpgradeType.ProductionCount)
            {
                var productionCountUpgrade = (ProductionCountUpgrade) productionUpgrade;
                ProductionCount *= productionCountUpgrade.Value;
                OnProductionCountChanged?.Invoke();
            }
        }
    }
}