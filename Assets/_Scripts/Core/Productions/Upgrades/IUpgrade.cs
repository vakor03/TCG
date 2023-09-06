namespace _Scripts.Core.Productions.Upgrades
{
    public interface IUpgrade
    {
        ProductionStats ApplyUpgrade(ProductionStats current);
    }
    
    public class AutomationUpgrade : IUpgrade
    {
        public ProductionStats ApplyUpgrade(ProductionStats current)
        {
            current.AutoProduction = true;
            return current;
        }
    }
    
    public class ProductionRateUpgrade : IUpgrade
    {
        private readonly float _rateIncrease;

        public ProductionRateUpgrade(float rateIncrease)
        {
            _rateIncrease = rateIncrease;
        }
        
        public ProductionStats ApplyUpgrade(ProductionStats current)
        {
            current.ProductionRate /= _rateIncrease;
            return current;
        }
    }
    
    public class ProductionCountUpgrade : IUpgrade
    {
        private readonly int _countIncrease;

        public ProductionCountUpgrade(int countIncrease)
        {
            _countIncrease = countIncrease;
        }
        
        public ProductionStats ApplyUpgrade(ProductionStats current)
        {
            current.ProductionCount *= _countIncrease;
            return current;
        }
    }
}