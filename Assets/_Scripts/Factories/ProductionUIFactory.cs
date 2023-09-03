using _Scripts.Core.Productions;
using _Scripts.ScriptableObjects;
using _Scripts.UI;
using UnityEngine;
using Zenject;

namespace _Scripts.Factories
{
    public class ProductionUIFactory : IProductionUIFactory
    {
        private readonly DiContainer _diContainer;
        private readonly ProductionUI _productionUITemplate;
        private readonly Transform _parentTransform;

        private ProductionUIFactory(DiContainer diContainer,
            ProductionUIContainerMB productionUIContainerMB)
        {
            _diContainer = diContainer;
            _productionUITemplate = productionUIContainerMB.ProductionUITemplate;
            _parentTransform = productionUIContainerMB.transform;
        }

        public ProductionUI Create(ProductionSO productionSO)
        {
             var productionUI = _diContainer
                 .InstantiatePrefabForComponent<ProductionUI>(_productionUITemplate, _parentTransform);
             productionUI.Init(productionSO);
             
             return productionUI;
        }
    }
}