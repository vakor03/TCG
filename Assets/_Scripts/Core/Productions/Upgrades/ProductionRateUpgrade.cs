namespace _Scripts.Core.Productions.Upgrades
{
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
}