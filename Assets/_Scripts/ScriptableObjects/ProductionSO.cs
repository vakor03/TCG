﻿using UnityEngine;

namespace _Scripts.ScriptableObjects
{
    [CreateAssetMenu()]
    public class ProductionSO : ScriptableObject
    {
        [field: SerializeField] public ResourceSO ConnectedResource { get; private set; }
        [field: SerializeField] public float ProductionRate { get; private set; }
        [field: SerializeField] public long ProductionCount { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public ResourceSO ProductionResourceType { get; private set; }
    }
}