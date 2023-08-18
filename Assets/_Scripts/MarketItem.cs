using System.Collections.Generic;
using _Scripts.ScriptableObjects;

namespace _Scripts
{
    public class MarketItem
    {
        public MarketItemSO MarketItemSO { get; }
        public ResourceSO OutputResource => MarketItemSO.OutputResource;
        public Dictionary<ResourceSO, long> PricePerUnit => MarketItemSO.PricePerUnit;

        public MarketItem(MarketItemSO marketItemSO)
        {
            MarketItemSO = marketItemSO;
        }
    }
}