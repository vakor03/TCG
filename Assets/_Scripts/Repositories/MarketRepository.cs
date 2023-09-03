using System.Collections.Generic;
using System.Linq;
using _Scripts.Helpers;
using _Scripts.ScriptableObjects;
using UnityEngine;

namespace _Scripts.Repositories
{
    public interface IMarketRepository: IRepository
    {
        MarketItem GetMarketItem(ResourceSO resourceSO);
        bool TryGetMarketItem(ResourceSO resourceSO, out MarketItem marketItem);
    }

    public class MarketRepository : IRepository
    {
        private const string MARKET_ITEM_PATH = "ScriptableObjects/MarketItems";

        private List<MarketItemSO> _marketItemSOs;
        private Dictionary<ResourceSO, MarketItemSO> _marketItemSOsMap;
        private Dictionary<ResourceSO, MarketItem> _marketItemsMap;

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

        public void Initialize()
        {
            AssembleResources();
        }

        public void Save()
        {
        }
    }
}