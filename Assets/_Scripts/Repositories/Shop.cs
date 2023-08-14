using System.Numerics;
using _Scripts.Helpers;
using _Scripts.ScriptableObjects;
using UnityEngine;

namespace _Scripts.Repositories
{
    public class Shop : StaticInstance<Shop>
    {
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