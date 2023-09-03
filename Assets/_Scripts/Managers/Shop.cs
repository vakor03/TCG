#region

using System.Numerics;
using _Scripts.Helpers;
using _Scripts.Interactors;
using _Scripts.Repositories;
using _Scripts.ScriptableObjects;
using _Scripts.UI;
using UnityEngine;
using Zenject;

#endregion

namespace _Scripts.Managers
{
    public class Shop : StaticInstance<Shop>
    {
        private InteractorsBase _interactorsBase;
        private MarketRepository _marketRepository;
        private RepositoriesBase _repositoriesBase;
        private ResourcesInteractor _resourcesInteractor;
        private ShopOptionManager _shopOptionManager;

        [Inject]
        public void Construct(RepositoriesBase repositoriesBase, 
            InteractorsBase interactorsBase,
            ShopOptionManager shopOptionManager)
        {
            _repositoriesBase = repositoriesBase;
            _interactorsBase = interactorsBase;
            _shopOptionManager = shopOptionManager;
        }

        private void Start()
        {
            _resourcesInteractor = _interactorsBase.GetInteractor<ResourcesInteractor>();
            _marketRepository = _repositoriesBase.GetRepository<MarketRepository>();
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
            if (_marketRepository.TryGetMarketItem(resourceSO, out var marketItemSO))
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
            if (_marketRepository.TryGetMarketItem(resourceSO, out var marketItemSO))
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