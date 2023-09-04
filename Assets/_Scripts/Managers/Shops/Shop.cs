#region

using System.Numerics;
using _Scripts.Core.MarketItems;
using _Scripts.Interactors;
using _Scripts.Repositories;
using _Scripts.ScriptableObjects;
using UnityEngine;

#endregion

namespace _Scripts.Managers.Shops
{
    public class Shop
    {
        private MarketItemDatabase _marketItemDatabase;
        private ResourcesInteractor _resourcesInteractor;
        private ShopOptionManager _shopOptionManager;

        private Shop(MarketItemDatabase marketItemDatabase, 
            ResourcesInteractor resourcesInteractor, 
            ShopOptionManager shopOptionManager)
        {
            _marketItemDatabase = marketItemDatabase;
            _resourcesInteractor = resourcesInteractor;
            _shopOptionManager = shopOptionManager;
        }

        public BigInteger CalculateCurrentBuyQuantity(ResourceSO resourceSO)
        {
            var currentShopOption = _shopOptionManager.CurrentShopOption;
            if (currentShopOption.Type == ShopOptionType.DefinedNumber)
            {
                return CalculateBuyQuantityForDefinedNumber(resourceSO, currentShopOption);
            }

            if (currentShopOption.Type == ShopOptionType.Percent)
            {
                return CalculateBuyQuantityForPercentType(resourceSO, currentShopOption);
            }

            Debug.LogError($"{currentShopOption.Type} not supported!");
            return BigInteger.Zero;
        }

        private BigInteger CalculateBuyQuantityForPercentType(ResourceSO resourceSO, ShopOption currentShopOption)
        {
            var maxPossibleBuyCount = FindMaxPossibleBuyCount(resourceSO);
            if (maxPossibleBuyCount == BigInteger.Zero)
            {
                return BigInteger.Zero;
            }
            else
            {
                return BigInteger.Max(BigInteger.One,
                    maxPossibleBuyCount * currentShopOption.Value / 100);
            }
        }

        private BigInteger CalculateBuyQuantityForDefinedNumber(ResourceSO resourceSO, ShopOption currentShopOption)
        {
            if (FindMaxPossibleBuyCount(resourceSO) > currentShopOption.Value)
            {
                return currentShopOption.Value;
            }

            return 0;
        }


        public BigInteger FindMaxPossibleBuyCount(ResourceSO resourceSO)
        {
            if (_marketItemDatabase.TryGetMarketItem(resourceSO, out var marketItemSO))
            {
                BigInteger max;
                bool firstValue = true;
                foreach (var (reqResourceSO, reqCount) in marketItemSO.PricePerUnit)
                {
                    var currentMax = _resourcesInteractor.GetResourceQuantity(reqResourceSO) / reqCount;
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

            return 0;
        }

        public bool TryBuyResource(ResourceSO resourceSO, BigInteger quantity)
        {
            if (_marketItemDatabase.TryGetMarketItem(resourceSO, out var marketItemSO))
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

        private void ExchangeResources(MarketItem marketItem, BigInteger quantity)
        {
            foreach (var (item, reqQuantityPerUnit) in marketItem.PricePerUnit)
            {
                BigInteger reqQuantityBigInteger = reqQuantityPerUnit * quantity;

                _resourcesInteractor.SpendResource(item, reqQuantityBigInteger);
            }

            _resourcesInteractor.AddResource(marketItem.OutputResource, quantity);
        }

        public bool CheckEnoughResourcesToBuy(MarketItem marketItemSO, BigInteger quantity)
        {
            foreach (var (item, reqQuantity) in marketItemSO.PricePerUnit)
            {
                if (!_resourcesInteractor.IsEnoughResource(item, reqQuantity * quantity))
                {
                    return false;
                }
            }

            return true;
        }
    }
}