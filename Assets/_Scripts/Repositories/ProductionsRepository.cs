using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Helpers;
using _Scripts.ScriptableObjects;
using _Scripts.Upgrades;
using UnityEngine;

namespace _Scripts.Repositories
{
    public class ProductionsRepository : StaticInstance<ProductionsRepository>
    {
        private const string PRODUCTIONS_PATH = "ScriptableObjects/Productions";
        private List<ProductionSO> _productionSOs;
        private Dictionary<ProductionSO, Production> _productionsMap;
        private Dictionary<ProductionSO, ProductionStats> _productionStatsMap;

        public List<ProductionSO> ProductionSOs => _productionSOs;


        protected override void Awake()
        {
            base.Awake();

            AssembleProductions();
        }

        private void AssembleProductions()
        {
            _productionSOs = Resources.LoadAll<ProductionSO>(PRODUCTIONS_PATH).ToList();
            _productionStatsMap = _productionSOs.ToDictionary(so => so, so => new ProductionStats(so));
            _productionsMap = _productionSOs.ToDictionary(so => so, so =>
            {
                return new Production(
                    _productionStatsMap[so],
                    so.ProductionResource,
                    so.ConnectedResource);
            });
        }

        private void Start()
        {
            foreach (var production in _productionsMap.Values)
            {
                production.OnStart();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                var testUpgrade = new AutoProductionUpgrade(new List<ProductionStats>
                    { _productionStatsMap[_productionSOs[0]] });
                testUpgrade.DoUpgrade();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                var testUpgrade = new ProductionRateUpgrade(new List<ProductionStats>
                    { _productionStatsMap[_productionSOs[0]] }, 2);
                testUpgrade.DoUpgrade();
            }
        }

        public Production GetProduction(ProductionSO productionsO)
        {
            return _productionsMap[productionsO];
        }
    }
}