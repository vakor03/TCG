using System;
using System.Collections.Generic;
using System.Numerics;
using _Scripts.Repositories;
using _Scripts.ScriptableObjects;

namespace _Scripts.Interactors
{
    // public interface IResourcesInteractor : IInteractor
    // {
    //     event Action<ResourceSO> OnResourceQuantityChanged;
    //     BigInteger GetResourceQuantity(ResourceSO resource);
    //     void SpendResource(ResourceSO resourceSO, BigInteger quantity);
    //     void AddResource(ResourceSO resourceSO, BigInteger quantity);
    //     bool IsEnoughResource(ResourceSO resourceSO, BigInteger quantity);
    //     void AddResources(Dictionary<ResourceSO, BigInteger> income);
    // }

    public class ResourcesInteractor : IInteractor
    {
        public static ResourcesInteractor Instance { get; private set; }
        public ResourcesInteractor()
        {
            Instance = this;
        }
        private ResourcesRepository _resourcesRepository { get; set; }

        public event Action<ResourceSO> OnResourceQuantityChanged;
        
        public void Initialize(RepositoriesBase repositoriesBase)
        {
            _resourcesRepository = repositoriesBase.GetRepository<ResourcesRepository>();
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