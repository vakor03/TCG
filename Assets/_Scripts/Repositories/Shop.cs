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
                new ShopOption("x1", ShopOptionType.DefinedNumber, 1),
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

        public BigInteger CalculateCurrentBuyQuantity(ResourceSO resourceSO)
        {
            if (CurrentShopOption.Type == ShopOptionType.DefinedNumber)
            {
                if (FindMaxPossibleBuyCount(resourceSO) > CurrentShopOption.Value)
                {
                    return CurrentShopOption.Value;
                }

                return 0;
            }

            if (CurrentShopOption.Type == ShopOptionType.Percent)
            {
                var maxPossibleBuyCount = FindMaxPossibleBuyCount(resourceSO);
                if (maxPossibleBuyCount == BigInteger.Zero)
                {
                    return BigInteger.Zero;
                }
                else
                {
                    return BigInteger.Max(BigInteger.One,
                        maxPossibleBuyCount * CurrentShopOption.Value / 100);
                }
            }
            
            Debug.LogError($"{CurrentShopOption.Type} not supported!");
            return BigInteger.Zero;
        }

        public BigInteger FindMaxPossibleBuyCount(ResourceSO resourceSO)
        {
            if (MarketRepository.Instance.TryGetMarketItem(resourceSO, out var marketItemSO))
            {
                BigInteger max;
                bool firstValue = true;
                foreach (var (reqResourceSO, reqCount) in marketItemSO.PricePerUnit)
                {
                    var currentMax = ResourcesRepository.Instance.GetResource(reqResourceSO).Count / reqCount;
                    if (firstValue)
                    {
                        max = currentMax;
                        firstValue = false;
                    }
                    else
                    {
                        if (currentMax < max)
                        {
                            max = currentMax;
                        }
                    }
                }

                return max;
            }
            else
            {
                return 0;
            }
        }

        public bool TryBuyResource(ResourceSO resourceSO, BigInteger quantity)
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
        
        

        private void ExchangeResources(MarketItemSO marketItemSO, BigInteger quantity)
        {
            foreach (var (item, reqQuantity) in marketItemSO.PricePerUnit)
            {
                BigInteger reqQuantityBigInteger = reqQuantity * quantity;
                
                ResourcesRepository.Instance.GetResource(item).Count -=
                    reqQuantityBigInteger;
            }

            ResourcesRepository.Instance.GetResource(marketItemSO.OutputResource).Count += quantity;
        }

        public bool CheckEnoughResourcesToBuy(MarketItemSO marketItemSO, BigInteger quantity)
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