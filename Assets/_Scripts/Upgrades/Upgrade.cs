using System;
using System.Collections.Generic;

namespace _Scripts.Upgrades
{
    public abstract class Upgrade
    {
        public abstract void DoUpgrade();
    }

    public abstract class ProductionUpgrade : Upgrade
    {
        public List<ProductionStats> ProductionsToUpgrade { get; set; }
        public UpgradeType UpgradeType { get; set; }

        protected ProductionUpgrade(List<ProductionStats> productionsToUpgrade, UpgradeType upgradeType)
        {
            ProductionsToUpgrade = productionsToUpgrade;
            UpgradeType = upgradeType;
        }

        public override void DoUpgrade()
        {
            foreach (var productionStats in ProductionsToUpgrade)
            {
                productionStats.UnlockUpgrade(this);
            }
        }
    }

    public class AutoProductionUpgrade : ProductionUpgrade
    {
        public AutoProductionUpgrade(List<ProductionStats> productionsToUpgrade) : base(productionsToUpgrade,
            UpgradeType.AutoProduction)
        {
        }
    }

    public class ProductionRateUpgrade : ProductionUpgrade
    {
        public long Value { get; set; }
        
        public ProductionRateUpgrade(List<ProductionStats> productionsToUpgrade, long value) : base(productionsToUpgrade,
            UpgradeType.ProductionRate)
        {
            Value = value;
        }
    }
}