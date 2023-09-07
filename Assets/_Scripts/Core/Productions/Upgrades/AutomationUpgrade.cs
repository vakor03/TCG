namespace _Scripts.Core.Productions.Upgrades
{
    public class AutomationUpgrade : IUpgrade
    {
        public ProductionStats ApplyUpgrade(ProductionStats current)
        {
            current.AutoProduction = true;
            return current;
        }
    }
}