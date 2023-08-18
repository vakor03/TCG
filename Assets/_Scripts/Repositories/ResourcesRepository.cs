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

        protected override void Awake()
        {
            base.Awake();

            Initialize();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

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