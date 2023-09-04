namespace _Scripts.Core.Productions.Upgrades
{
    public interface IUpgrade
    {
        ProductionStats ApplyUpgrade(ProductionStats current);
    }
}