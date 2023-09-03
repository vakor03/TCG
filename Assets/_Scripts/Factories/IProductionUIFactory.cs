using _Scripts.ScriptableObjects;
using _Scripts.UI;

namespace _Scripts.Factories
{
    public interface IProductionUIFactory
    {
        ProductionUI Create(ProductionSO productionSO);
    }
}