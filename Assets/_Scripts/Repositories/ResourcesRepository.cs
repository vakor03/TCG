#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using _Scripts.Helpers;
using _Scripts.ScriptableObjects;
using UnityEngine;

#endregion

namespace _Scripts.Repositories
{
    public class ResourcesRepository : StaticInstance<ResourcesRepository>, IRepository
    {
        private const string PLAYER_PREFS_RESOURCES_PREFIX = "RES_KEY_";
        private const string RESOURCES_PATH = "ScriptableObjects/Resources";
        private List<ResourceSO> _resourceSOs;


        private Dictionary<ResourceSO, BigInteger> _resourcesQuantityMap;
        // private Dictionary<ResourceSO, Resource> _resourcesMap;

        protected override void Awake()
        {
            base.Awake();

            // AssembleResources();
            Initialize();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        private void AssembleResources()
        {
            _resourceSOs = Resources.LoadAll<ResourceSO>(RESOURCES_PATH).ToList();
            // _resourcesMap = _resourceSOs.ToDictionary(so => so, so => new Resource(so));
        }

        // public Resource GetResource(ResourceSO resourceSO)
        // {
        //     return _resourcesMap[resourceSO];
        // }

        // public void IncreaseResourceCount(ResourceSO productionResource, BigInteger productionCount)
        // {
        //     _resourcesMap[productionResource].Count += productionCount;
        // }

        public void Initialize()
        {
            _resourceSOs = Resources.LoadAll<ResourceSO>(RESOURCES_PATH).ToList();

            _resourcesQuantityMap = new Dictionary<ResourceSO, BigInteger>();
            foreach (var resourceSO in _resourceSOs)
            {
                var resourceCount =
                    PlayerPrefs.GetString(PLAYER_PREFS_RESOURCES_PREFIX + resourceSO.Name, "100");
                
               SetResourceQuantity(resourceSO, BigInteger.Parse(resourceCount));
            }
        }

        public void Save()
        {
            foreach (var (resourceSO, quantity) in _resourcesQuantityMap)
            {
                PlayerPrefs.SetString(PLAYER_PREFS_RESOURCES_PREFIX + resourceSO.Name, quantity.ToString());
            }

            PlayerPrefs.Save();
        }

        public event Action OnAnyResourceQuantityChanged;
        public event Action<ResourceSO> OnResourceQuantityChanged;

        public BigInteger GetResourceQuantity(ResourceSO resource)
        {
            return _resourcesQuantityMap[resource];
        }

        public void SetResourceQuantity(ResourceSO resource, BigInteger newValue)
        {
            if (_resourcesQuantityMap.ContainsKey(resource)
                && GetResourceQuantity(resource) == newValue)
            {
                return;
            }

            _resourcesQuantityMap[resource] = newValue;

            OnAnyResourceQuantityChanged?.Invoke();
            OnResourceQuantityChanged?.Invoke(resource);
        }

        public void SpendResource(ResourceSO resourceSO, BigInteger quantity)
        {
            SetResourceQuantity(resourceSO, GetResourceQuantity(resourceSO) - quantity);
        }
        
        public void AddResource(ResourceSO resourceSO, BigInteger quantity)
        {
            SetResourceQuantity(resourceSO, GetResourceQuantity(resourceSO) + quantity);
        }
        
        public bool IsEnoughResource(ResourceSO resourceSO, BigInteger quantity)
        {
            return GetResourceQuantity(resourceSO) >= quantity;
        }
    }
}