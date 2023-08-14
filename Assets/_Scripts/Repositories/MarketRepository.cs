using System.Collections.Generic;
using System.Linq;
using _Scripts.Helpers;
using _Scripts.ScriptableObjects;
using UnityEngine;

namespace _Scripts.Repositories
{
    public class MarketRepository : StaticInstance<MarketRepository>
    {
        private const string MARKET_ITEM_PATH = "ScriptableObjects/MarketItems";

        private List<MarketItemSO> _marketItemSOs;
        private Dictionary<ResourceSO, MarketItemSO> _marketItemsMap;

        protected override void Awake()
        {
            base.Awake();

            AssembleResources();
        }

        public MarketItemSO GetMarketItem(ResourceSO resourceSO)
        {
            return _marketItemsMap[resourceSO];
        }

        public bool TryGetMarketItem(ResourceSO resourceSO, out MarketItemSO marketItemSO)
        {
            return _marketItemsMap.TryGetValue(resourceSO, out marketItemSO);
        }

        private void AssembleResources()
        {
            _marketItemSOs = Resources.LoadAll<MarketItemSO>(MARKET_ITEM_PATH).ToList();
            _marketItemsMap = _marketItemSOs.ToDictionary(so => so.OutputResource, so => so);
        }
    }
}