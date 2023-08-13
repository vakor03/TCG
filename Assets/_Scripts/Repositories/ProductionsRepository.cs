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

        public Production GetResourceProducer(ProductionSO productionsO)
        {
            return _productionsMap[productionsO];
        }
    }
}