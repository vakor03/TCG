using System.Collections.Generic;
using System.Linq;
using _Scripts.Factories;
using _Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace _Scripts.Repositories
{
    public class ProductionContainer : IInitializable
    {
        private const string PRODUCTIONS_PATH = "ScriptableObjects/Productions";
        private List<ProductionSO> _productionSOs;
        private Dictionary<ProductionSO, Production> _productionsMap;
        private Dictionary<ProductionSO, ProductionStats> _productionStatsMap;

        private readonly IProductionFactory _productionFactory;
        public List<ProductionSO> ProductionSOs => _productionSOs;

        public ProductionContainer(IProductionFactory productionFactory)
        {
            _productionFactory = productionFactory;
        }

        private void AssembleProductions()
        {
            _productionSOs = Resources.LoadAll<ProductionSO>(PRODUCTIONS_PATH).ToList();
            _productionStatsMap = _productionSOs.ToDictionary(so => so, so => new ProductionStats(so));
            _productionsMap = _productionSOs.ToDictionary(so => so, so =>
            {
                return _productionFactory.Create(
                    _productionStatsMap[so],
                    so.ProductionResource,
                    so.ConnectedResource);
            });
        }

        public void OnStart()
        {
            foreach (var production in _productionsMap.Values)
            {
                production.OnStart();
            }
        }

        public Production GetProduction(ProductionSO productionsO)
        {
            return _productionsMap[productionsO];
        }

        public ProductionStats GetProductionStats(ProductionSO productionSO)
        {
            return _productionStatsMap[productionSO];
        }

        public void Initialize()
        {
            AssembleProductions();
        }
    }
}