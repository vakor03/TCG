using _Scripts.Core.Productions;
using _Scripts.ScriptableObjects;

namespace _Scripts.Factories
{
    public interface IProductionFactory
    {
        Production Create(ProductionStats productionStats,
            ResourceSO productedResource,
            ResourceSO connectedResource);
    }
}