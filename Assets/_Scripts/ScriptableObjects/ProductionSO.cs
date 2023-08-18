using UnityEngine;

namespace _Scripts.ScriptableObjects
{
    [CreateAssetMenu()]
    public class ProductionSO : ScriptableObject
    {
        [field: SerializeField] public ResourceSO ConnectedResource { get; private set; }
        [field: SerializeField] public float BaseProductionRate { get; private set; }
        [field: SerializeField] public long ProductionCount { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public ResourceSO ProductionResource { get; private set; }
    }
}