using _Scripts.Interactors;
using _Scripts.Repositories;
using _Scripts.ScriptableObjects;

namespace _Scripts.Factories
{
    public class ProductionFactory : IProductionFactory
    {
        private readonly ResourcesInteractor _resourcesInteractor;

        private ProductionFactory(ResourcesInteractor resourcesInteractor)
        {
            _resourcesInteractor = resourcesInteractor;
        }

        public Production Create(ProductionStats productionStats,
            ResourceSO productedResource,
            ResourceSO connectedResource)
        {
            return new Production(productionStats,
                productedResource,
                connectedResource,
                _resourcesInteractor);
        }
    }
}