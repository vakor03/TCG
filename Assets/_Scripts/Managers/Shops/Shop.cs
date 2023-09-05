#region

using System.Linq;
using System.Numerics;
using _Scripts.Core.MarketItems;
using _Scripts.Helpers;
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
            if (!_marketItemDatabase.CheckIfMarketItemExists(resourceSO))
            {
                return BigInteger.Zero;
            }

            MarketItem marketItemSO = _marketItemDatabase.GetMarketItem(resourceSO);
            BigInteger max = marketItemSO.PricePerUnit.Max(pair =>  _resourcesInteractor.GetResourceQuantity(pair.Key) / pair.Value);
            
            return max;
        }

        public bool CanBuyResource(ResourceSO resourceSO, BigInteger quantity)
        {
            if (!_marketItemDatabase.CheckIfMarketItemExists(resourceSO))
            {
                return false;
            }

            MarketItem marketItemSO = _marketItemDatabase.GetMarketItem(resourceSO);
            return CheckEnoughResourcesToBuy(marketItemSO, quantity);
        }
        
        public void BuyResource(ResourceSO resourceSO, BigInteger quantity)
        {
            MarketItem marketItem = _marketItemDatabase.GetMarketItem(resourceSO);
            
            ExchangeResources(marketItem, quantity);
            LogBuy(resourceSO, quantity);
        }
        
        private void LogBuy(ResourceSO resourceSO, BigInteger quantity)
        {
            Debug.Log($"Bought {quantity.ToScientificNotationString()} {resourceSO.Name}");
        }
      
        private void ExchangeResources(MarketItem marketItem, BigInteger quantity)
        {
            foreach (var (item, reqQuantityPerUnit) in marketItem.PricePerUnit)
            {
                BigInteger totalRequiredQuantity = reqQuantityPerUnit * quantity;

                _resourcesInteractor.SpendResource(item, totalRequiredQuantity);
            }

            _resourcesInteractor.AddResource(marketItem.OutputResource, quantity);
        }

        public bool CheckEnoughResourcesToBuy(MarketItem marketItemSO, BigInteger quantity)
        {
            // foreach (var (item, reqQuantity) in marketItemSO.PricePerUnit)
            // {
            //     if (!_resourcesInteractor.IsEnoughResource(item, reqQuantity * quantity))
            //     {
            //         return false;
            //     }
            // }
            //
            // return true;
            
            return marketItemSO.PricePerUnit.All(pair => _resourcesInteractor.IsEnoughResource(pair.Key, pair.Value * quantity));
        }
    }
}