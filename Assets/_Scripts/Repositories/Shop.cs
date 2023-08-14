using System;
using System.Numerics;
using _Scripts.Helpers;
using _Scripts.ScriptableObjects;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Repositories
{
    public class Shop : StaticInstance<Shop>
    {
        private ShopOption[] _availableOptions;
        public event Action OnShopOptionChanged;
        public ShopOption CurrentShopOption { get; private set; }
        private int _currentShopOptionIndex;

        protected override void Awake()
        {
            base.Awake();

            _availableOptions = new[]
            {
                new ShopOption("x1", ShopOptionType.Additive, 1),
                new ShopOption("10%", ShopOptionType.Percent, 10),
                new ShopOption("50%", ShopOptionType.Percent, 50),
                new ShopOption("MAX", ShopOptionType.Percent, 100),
            };

            _currentShopOptionIndex = 0;
            CurrentShopOption = _availableOptions[_currentShopOptionIndex];
        }

        public void ChooseNextShopOption()
        {
            _currentShopOptionIndex = (_currentShopOptionIndex + 1) % _availableOptions.Length;
            CurrentShopOption = _availableOptions[_currentShopOptionIndex];
            OnShopOptionChanged?.Invoke();
        }

        public bool TryBuyResource(ResourceSO resourceSO, int quantity)
        {
            if (MarketRepository.Instance.TryGetMarketItem(resourceSO, out var marketItemSO))
            {
                if (!CheckEnoughResourcesToBuy(marketItemSO, quantity))
                {
                    return false;
                }

                ExchangeResources(marketItemSO, quantity);
                return true;
            }
            else
            {
                Debug.LogError($"No market item for {resourceSO.Name}");
                return false;
            }
        }

        private void ExchangeResources(MarketItemSO marketItemSO, int quantity)
        {
            foreach (var (item, reqQuantity) in marketItemSO.PricePerUnit)
            {
                BigInteger reqQuantityBigInteger = reqQuantity * quantity;
                
                ResourcesRepository.Instance.GetResource(item).Count -=
                    reqQuantityBigInteger;
            }

            ResourcesRepository.Instance.GetResource(marketItemSO.OutputResource).Count += quantity;
        }

        public bool CheckEnoughResourcesToBuy(MarketItemSO marketItemSO, int quantity)
        {
            foreach (var (item, reqQuantity) in marketItemSO.PricePerUnit)
            {
                if (ResourcesRepository.Instance.GetResource(item).Count <
                    reqQuantity * quantity)
                {
                    return false;
                }
            }

            return true;
        }
    }
}