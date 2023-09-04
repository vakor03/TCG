namespace _Scripts.Core.Productions
{
    public interface IUpgrade
    {
        ProductionStats ApplyUpgrade(ProductionStats current);
    }
}