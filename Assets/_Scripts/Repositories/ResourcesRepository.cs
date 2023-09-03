#region

using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using _Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

#endregion

namespace _Scripts.Repositories
{
    public class ResourcesRepository : IInitializable
    {
        private const string PLAYER_PREFS_RESOURCES_PREFIX = "RES_KEY_";
        private const string RESOURCES_PATH = "ScriptableObjects/Resources";

        private List<ResourceSO> _resourceSOs;
        private Dictionary<ResourceSO, BigInteger> _resourcesQuantityMap;

        public Dictionary<ResourceSO, BigInteger> ResourcesQuantityMap => _resourcesQuantityMap;
        
        public void Initialize()
        {
            _resourceSOs = Resources.LoadAll<ResourceSO>(RESOURCES_PATH).ToList();

            _resourcesQuantityMap = new Dictionary<ResourceSO, BigInteger>();
            foreach (var resourceSO in _resourceSOs)
            {
                var resourceCount =
                    PlayerPrefs.GetString(GetPlayerPrefsKey(resourceSO), "100");
                
               SetResourceQuantity(resourceSO, BigInteger.Parse(resourceCount));
            }
        }

        public void Save()
        {
            foreach (var (resourceSO, quantity) in _resourcesQuantityMap)
            {
                PlayerPrefs.SetString(GetPlayerPrefsKey(resourceSO), quantity.ToString());
            }

            PlayerPrefs.Save();
        }

        private string GetPlayerPrefsKey(ResourceSO resourceSO)
        {
            return PLAYER_PREFS_RESOURCES_PREFIX + resourceSO.Name;
        }

        public BigInteger GetResourceQuantity(ResourceSO resource)
        {
            return _resourcesQuantityMap[resource];
        }

        private void SetResourceQuantity(ResourceSO resource, BigInteger newValue)
        {
            if (_resourcesQuantityMap.ContainsKey(resource)
                && GetResourceQuantity(resource) == newValue)
            {
                return;
            }

            _resourcesQuantityMap[resource] = newValue;
        }
    }
}