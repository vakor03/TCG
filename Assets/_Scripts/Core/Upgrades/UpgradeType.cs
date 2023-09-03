using _Scripts.Core.Productions;

namespace _Scripts.Core.Upgrades
{
    public enum UpgradeType
    {
        ProductionRate,
        ProductionCount,
        Speed,
        AutoProduction,
    }

    public interface IProductionUpgrade
    {
        public UpgradeType Type { get; }
        public void ApplyUpgrade(Production context);
    }
}