using System.Collections.Generic;
using System.Linq;
using _Scripts.Core.Productions;
using _Scripts.Factories;
using _Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace _Scripts.Repositories
{
    public class ProductionDatabase : IInitializable
    {
        private const string PRODUCTIONS_PATH = "ScriptableObjects/Productions";
        private List<ProductionSO> _productionSOs;
        public Dictionary<ProductionSO, Producer> ProducersMap => _productionsMap;
        private Dictionary<ProductionSO, Producer> _productionsMap;

        private readonly IProductionFactory _productionFactory;
        public List<ProductionSO> ProductionSOs => _productionSOs;

        private ProductionDatabase(IProductionFactory productionFactory)
        {
            _productionFactory = productionFactory;
        }

        private void AssembleProductions()
        {
            _productionSOs = Resources.LoadAll<ProductionSO>(PRODUCTIONS_PATH).ToList();
            _productionsMap = _productionSOs.ToDictionary(so => so, so =>
            {
                return _productionFactory.Create(so);
            });
        }

        public Producer GetProducer(ProductionSO productionsO)
        {
            return _productionsMap[productionsO];
        }

        public void Initialize()
        {
            AssembleProductions();
        }
    }
}