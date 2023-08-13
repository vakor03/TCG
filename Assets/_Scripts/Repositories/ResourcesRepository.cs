using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using _Scripts.Helpers;
using _Scripts.ScriptableObjects;
using UnityEngine;

namespace _Scripts.Repositories
{
    public class ResourcesRepository : StaticInstance<ResourcesRepository>
    {
        private const string RESOURCES_PATH = "ScriptableObjects/Resources";
        private List<ResourceSO> _resourceSOs;
        private Dictionary<ResourceSO, Resource> _resourcesMap;

        protected override void Awake()
        {
            base.Awake();

            AssembleResources();
        }

        private void AssembleResources()
        {
            _resourceSOs = Resources.LoadAll<ResourceSO>(RESOURCES_PATH).ToList();
            _resourcesMap = _resourceSOs.ToDictionary(so => so, so => new Resource(so));
        }

        public Resource GetResourceInfo(ResourceSO resourceSO)
        {
            return _resourcesMap[resourceSO];
        }

        public void IncreaseResourceCount(ResourceSO productionResource, BigInteger productionCount)
        {
            _resourcesMap[productionResource].Count += productionCount;
        }
    }
}