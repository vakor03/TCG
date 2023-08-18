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
        private Dictionary<ResourceSO, MarketItemSO> _marketItemSOsMap;
        private Dictionary<ResourceSO, MarketItem> _marketItemsMap;

        protected override void Awake()
        {
            base.Awake();

            AssembleResources();
        }

        public MarketItem GetMarketItem(ResourceSO resourceSO)
        {
            return _marketItemsMap[resourceSO];
        }

        public bool TryGetMarketItem(ResourceSO resourceSO, out MarketItem marketItem)
        {
            return _marketItemsMap.TryGetValue(resourceSO, out marketItem);
        }

        private void AssembleResources()
        {
            _marketItemSOs = Resources.LoadAll<MarketItemSO>(MARKET_ITEM_PATH).ToList();
            _marketItemSOsMap = _marketItemSOs.ToDictionary(so => so.OutputResource, so => so);
            _marketItemsMap = _marketItemSOs.ToDictionary(so => so.OutputResource, so => new MarketItem(so));
        }
    }
}