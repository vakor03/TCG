using System;

namespace _Scripts.Upgrades
{
    public class ProductionModifiers
    {
        private int _productionCountModifier;
        private int _productionRateModifier;
        private int _priceModifier;
        private float _criticalChanceModifier;
        private int _criticalCountModifier;

        public int ProductionCountModifier
        {
            get => _productionCountModifier;
            set
            {
                _productionCountModifier = value;
                OnProductionCountModifierChanged?.Invoke(value);
            }
        }

        public int ProductionRateModifier
        {
            get => _productionRateModifier;
            set
            {
                _productionRateModifier = value;
                OnProductionRateModifierChanged?.Invoke(value);
            }
        }

        public int PriceModifier
        {
            get => _priceModifier;
            set
            {
                _priceModifier = value;
                OnPriceModifierChanged?.Invoke(value);
            }
        }

        public float CriticalChanceModifier
        {
            get => _criticalChanceModifier;
            set
            {
                _criticalChanceModifier = value;
                OnCriticalChanceModifierChanged?.Invoke(value);
            }
        }

        public int CriticalCountModifier
        {
            get => _criticalCountModifier;
            set
            {
                _criticalCountModifier = value;
                OnCriticalCountModifierChanged?.Invoke(value);
            }
        }

        public event Action<int> OnProductionCountModifierChanged;
        public event Action<int> OnProductionRateModifierChanged;
        public event Action<int> OnPriceModifierChanged;
        public event Action<float> OnCriticalChanceModifierChanged;
        public event Action<int> OnCriticalCountModifierChanged;

        public ProductionModifiers()
        {
            ProductionCountModifier = 1;
            ProductionRateModifier = 1;
            PriceModifier = 1;
            CriticalChanceModifier = 0f;
            CriticalCountModifier = 2;
        }
    }
}