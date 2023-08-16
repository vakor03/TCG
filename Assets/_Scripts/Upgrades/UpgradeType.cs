namespace _Scripts.Upgrades
{
    public enum UpgradeType
    {
        Power,
        Discount,
        Speed,
    }

    public interface IProductionUpgrade
    {
        public UpgradeType Type { get; }
        public void ApplyUpgrade(Production context);
    }
}