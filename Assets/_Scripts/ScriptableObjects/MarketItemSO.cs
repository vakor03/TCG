using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Create MarketItemSO", fileName = "MarketItemSO", order = 0)]
    public class MarketItemSO : ScriptableObject
    {
        [field: SerializeField] public ResourceSO OutputResource { get; private set; }

        [field: SerializeField]
        [SerializedDictionary("Resource", "Count")]
        public SerializedDictionary<ResourceSO, long> PricePerUnit { get; private set; }
    }
}