using _Scripts.Enums;
using UnityEngine;

namespace _Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Create ResourceSO", fileName = "_ResourceSO", order = 0)]
    public class ResourceSO : ScriptableObject
    {
        [field: SerializeField] public ResourceType Type { get; private set; }
        [field: SerializeField] public float BasicProductionRate { get; private set; }
        [field: SerializeField] public float BasicProductionCount { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public ResourceType ProductionResourceType { get; private set; }
    }
}