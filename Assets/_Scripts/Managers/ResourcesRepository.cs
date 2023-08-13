using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using _Scripts.Enums;
using _Scripts.Helpers;
using _Scripts.ScriptableObjects;
using UnityEngine;

namespace _Scripts.Managers
{
    public class ResourcesRepository : StaticInstance<ResourcesRepository>
    {
        [SerializeField] private List<ResourceSO> resources;

        private Dictionary<ResourceType, Resource> _resourcesMap;

        public Resource GetResource(ResourceType type)
        {
            return _resourcesMap[type];
        }

        protected override void Awake()
        {
            base.Awake();

            InitResourcesMap();
        }

        private void InitResourcesMap()
        {
            _resourcesMap = resources.ToDictionary(res => res.Type, res => new Resource(res));
        }

        public void IncreaseResourceCount(ResourceType resourceType, BigInteger value)
        {
            _resourcesMap[resourceType].IncreaseCount(value);
        }
    }
}