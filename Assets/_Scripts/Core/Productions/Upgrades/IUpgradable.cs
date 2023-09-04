namespace _Scripts.Core.Productions
{
    public interface IUpgradable
    {
        void AddUpgrade(IUpgrade upgrade);
        void RemoveUpgrade(IUpgrade upgrade);
    }
}