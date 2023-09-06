using _Scripts.Core.Productions;
using _Scripts.Interactors;
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
        
        public Producer Create(ProductionSO productionSO)
        {
            return new Producer(productionSO, _resourcesInteractor);
        }
    }
}