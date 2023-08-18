using System;
using System.Collections.Generic;
using System.Numerics;
using _Scripts.ScriptableObjects;
using _Scripts.Upgrades;

namespace _Scripts
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