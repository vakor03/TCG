namespace _Scripts.Core.Productions.Upgrades
{
    public interface IUpgradable
    {
        void AddUpgrade(IUpgrade upgrade);
        void RemoveUpgrade(IUpgrade upgrade);
    }
}