using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Helpers;
using _Scripts.ScriptableObjects;
using UnityEngine;

namespace _Scripts.Repositories
{
    public class ProductionsRepository : StaticInstance<ProductionsRepository>
    {
        private const string PRODUCTIONS_PATH = "ScriptableObjects/Productions";
        private List<ProductionSO> _productionSOs;
        private Dictionary<ProductionSO, Production> _productionsMap;

        public List<ProductionSO> ProductionSOs => _productionSOs;

        protected override void Awake()
        {
            base.Awake();

            AssembleProductions();
        }

        private void AssembleProductions()
        {
            _productionSOs = Resources.LoadAll<ProductionSO>(PRODUCTIONS_PATH).ToList();
            _productionsMap = _productionSOs.ToDictionary(so => so, so => new Production(so));
        }

        private void Start()
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
    }
}