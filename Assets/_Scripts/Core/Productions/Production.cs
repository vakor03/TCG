#region

using System;
using System.Collections.Generic;
using System.Numerics;
using _Scripts.Interactors;
using _Scripts.ScriptableObjects;
using MEC;

#endregion

namespace _Scripts.Core.Productions
{
    public class Production
    {
        private ResourceSO _connectedResourceSO;
        private State _currentState;
        private IProducer _producer;
        private ResourceSO _productionResourceSO;
        public ResourcesInteractor _resourcesInteractor;

        public Production(IProducer producer,
            ResourceSO productionResourceSO,
            ResourceSO connectedResourceSO,
            ResourcesInteractor resourcesInteractor)
        {
            _producer = producer;
            _productionResourceSO = productionResourceSO;
            _connectedResourceSO = connectedResourceSO;
            _resourcesInteractor = resourcesInteractor;
        }

        private bool IsRunning => _currentState == State.Running;

        public event Action OnStarted;
        public event Action OnFinished;

        public void StartProduction()
        {
            if (IsRunning)
            {
                return;
            }

            StartOneTimeProductionCoroutine();
        }

        private void StartOneTimeProductionCoroutine()
        {
            Timing.RunCoroutine(OneTimeProductionCoroutine());
        }

        private IEnumerator<float> OneTimeProductionCoroutine()
        {
            _currentState = State.Running;
            OnStarted?.Invoke();

            yield return Timing.WaitForSeconds(_producer.CurrentStats.ProductionRate);

            _resourcesInteractor.AddResource(_productionResourceSO, GetProductionCount());

            OnFinished?.Invoke();
            _currentState = State.Stopped;
        }

        private BigInteger GetProductionCount()
        {
            return _producer.CurrentStats.ProductionCount
                   * _resourcesInteractor.GetResourceQuantity(_connectedResourceSO);
        }

        private enum State
        {
            Stopped,
            Running,
        }
    }
}