using System;
using _Scripts.UI;

namespace _Scripts.Managers
{
    public class ShopOptionManager
    {
        private ShopOption[] _availableOptions;
        private int _currentShopOptionIndex;
        public ShopOption CurrentShopOption { get; private set; }

        public ShopOptionManager()
        {
            _availableOptions = new[]
            {
                new ShopOption("x1", ShopOptionType.DefinedNumber, 1),
                new ShopOption("10%", ShopOptionType.Percent, 10),
                new ShopOption("50%", ShopOptionType.Percent, 50),
                new ShopOption("MAX", ShopOptionType.Percent, 100),
            };

            _currentShopOptionIndex = 0;
            CurrentShopOption = _availableOptions[_currentShopOptionIndex];
        }

        public event Action OnShopOptionChanged;

        public void ChooseNextShopOption()
        {
            _currentShopOptionIndex = (_currentShopOptionIndex + 1) % _availableOptions.Length;
            CurrentShopOption = _availableOptions[_currentShopOptionIndex];
            OnShopOptionChanged?.Invoke();
        }
    }
}