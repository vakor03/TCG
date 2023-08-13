using UnityEngine;

namespace _Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Create ResourceSO", fileName = "ResourceSO", order = 0)]
    public class ResourceSO : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
    }
}