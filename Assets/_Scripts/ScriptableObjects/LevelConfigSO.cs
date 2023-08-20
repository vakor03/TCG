using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Create LevelConfigSO", fileName = "LevelConfigSO", order = 0)]
    public class LevelConfigSO : ScriptableObject
    {
        [field:SerializeField] public int Level { get; private set; }
        [field:SerializeField] public List<ProductionSO> ProductionsAvailable { get; private set; }
    }
}