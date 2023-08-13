using System;
using System.Collections.Generic;
using _Scripts.Repositories;
using _Scripts.ScriptableObjects;
using MEC;

namespace _Scripts
{
    public class Production
    {
        public event Action OnProductionFinished;
        public event Action OnProductionStarted;

        public bool AutoProduction { get; private set; }

        private State _currentState;
        private ProductionSO _productionSO;
        
        public Production(ProductionSO productionSO)
        {
            _currentState = State.Stopped;
            _productionSO = productionSO;
        }

        public void StartProduction()
        {
            if (_currentState == State.Production)
            {
                return;
            }

            Timing.RunCoroutine(ProductionCoroutine());
        }

        private IEnumerator<float> ProductionCoroutine()
        {
            _currentState = State.Production;
            OnProductionStarted?.Invoke();

            yield return Timing.WaitForSeconds(_productionSO.ProductionRate);

            AddResourceToRepository();
            
            OnProductionFinished?.Invoke();
            _currentState = State.Stopped;
        }

        private void AddResourceToRepository()
        {
            var productionResource = _productionSO.ProductionResource;
            var connectedResource = _productionSO.ConnectedResource;
            
            var productionCount = _productionSO.ProductionCount * 
                                  ResourcesRepository.Instance.GetResource(connectedResource).Count;
            
            ResourcesRepository.Instance.IncreaseResourceCount(productionResource, productionCount);
        }

        private enum State
        {
            Stopped,
            Production
        }
    }
}