namespace _Scripts.Core.Productions.Upgrades
{
    public class ProductionCountUpgrade : IUpgrade, IHaveLevel
    {
        private int _baseCount;
        private int _currentCount;
        private int _increasePerLevel;
        private int _level;

        public ProductionCountUpgrade(int baseCount)
        {
            _baseCount = baseCount;
            _currentCount = baseCount;
            _increasePerLevel = 2;
        }

        public void ChangeLevel(int newValue)
        {
            if (_level == newValue)
            {
                return;
            }
            
            _level = newValue;
            RecalculateCurrentCount();
        }

        public int Level => _level;

        public ProductionStats ApplyUpgrade(ProductionStats current)
        {
            current.ProductionCount *= _currentCount;
            return current;
        }

        private void RecalculateCurrentCount()
        {
            _currentCount = _baseCount;
            for (int i = 0; i < _level; i++)
            {
                _currentCount *= _increasePerLevel;
            }
        }
    }
}