using System;
using System.Collections.Generic;
using System.Numerics;
using _Scripts.Repositories;
using _Scripts.ScriptableObjects;

namespace _Scripts.Interactors
{
    public class ResourcesInteractor : IInteractor
    {
        private ResourcesRepository _resourcesRepository;
        
        public event Action<ResourceSO> OnResourceQuantityChanged;

        public void Initialize()
        {
            _resourcesRepository = GameManager.Instance.RepositoriesBase
                .GetRepository<ResourcesRepository>();
        }

        public BigInteger GetResourceQuantity(ResourceSO resource)
        {
            return _resourcesRepository.ResourcesQuantityMap[resource];
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

        private void SetResourceQuantity(ResourceSO resource, BigInteger newValue)
        {
            if (_resourcesRepository.ResourcesQuantityMap.ContainsKey(resource)
                && GetResourceQuantity(resource) == newValue)
            {
                return;
            }

            _resourcesRepository.ResourcesQuantityMap[resource] = newValue;

            OnResourceQuantityChanged?.Invoke(resource);
        }

        public void AddResources(Dictionary<ResourceSO, BigInteger> income)
        {
            foreach (var (resourceSO, quantity) in income)
            {
                AddResource(resourceSO, quantity);
            }
        }
    }
}