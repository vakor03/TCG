using _Scripts.Core.Productions;
using UnityEngine;

namespace _Scripts.ScriptableObjects
{
    [CreateAssetMenu()]
    public class ProductionSO : ScriptableObject
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public ResourceSO ConnectedResource { get; private set; }
        [field: SerializeField] public ResourceSO ProductionResource { get; private set; }
        [field: SerializeField] public SerializableProductionStats BaseStats { get; private set; }
    }
}