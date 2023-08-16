using UnityEngine;

namespace _Scripts.ScriptableObjects
{
    public class ProductionUpgradeSO : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string UpgradeName { get; private set; }
        [field: SerializeField] public string Description { get; private set; }

    }
}