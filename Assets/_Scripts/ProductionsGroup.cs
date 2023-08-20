using _Scripts.Helpers;
using _Scripts.ScriptableObjects;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts
{
    public class ProductionsGroup : StaticInstance<ProductionsGroup>
    {
        [SerializeField] private ProductionUI productionUITemplate;

        public void AddProduction(ProductionSO productionSO)
        {
            var productionUI = Instantiate(productionUITemplate, transform);
            productionUI.Init(productionSO);
        }
    }
}