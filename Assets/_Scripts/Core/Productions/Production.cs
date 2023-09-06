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
        private IProducer _producer;
        private ResourceSO _productionResourceSO;
        private ResourcesInteractor _resourcesInteractor;
        private bool _autoProduction;

        public Production(IProducer producer,
            ResourceSO productionResourceSO,
            ResourceSO connectedResourceSO,
            ResourcesInteractor resourcesInteractor)
        {
            _producer = producer;
            _productionResourceSO = productionResourceSO;
            _connectedResourceSO = connectedResourceSO;
            _resourcesInteractor = resourcesInteractor;
            
            _producer.OnStatsChanged += ProducerOnStatsChanged;
        }

        private void ProducerOnStatsChanged()
        {
            if (IsAutoProductionChanged())
            {
                ToggleAutoProduction();
            }

            bool IsAutoProductionChanged()
            {
                return _autoProduction != _producer.CurrentStats.AutoProduction;
            }
        }
        
        private void ToggleAutoProduction()
        {
            _autoProduction = !_autoProduction;

            if (_autoProduction)
            {
                StartAutoProduction();
            }
            else
            {
                StopAutoProduction();
            }
        }

        private void StopAutoProduction()
        {
            OnFinished -= StartOneTimeProductionCoroutine;
        }

        private void StartAutoProduction()
        {
            OnFinished += StartOneTimeProductionCoroutine;

            if (!IsRunning)
            {
                StartOneTimeProductionCoroutine();
            }
        }

        public bool IsRunning { get; private set; }

        public event Action OnStarted;
        public event Action OnFinished;

        public void StartProduction()
        {
            StartOneTimeProductionCoroutine();
        }

        private void StartOneTimeProductionCoroutine()
        {
            Timing.RunCoroutine(OneTimeProductionCoroutine());
        }

        private IEnumerator<float> OneTimeProductionCoroutine()
        {
           IsRunning = true;
            OnStarted?.Invoke();

            yield return Timing.WaitForSeconds(_producer.CurrentStats.ProductionRate);

            _resourcesInteractor.AddResource(_productionResourceSO, GetProductionCount());

            OnFinished?.Invoke();
            IsRunning = false;
        }

        private BigInteger GetProductionCount()
        {
            return _producer.CurrentStats.ProductionCount
                   * _resourcesInteractor.GetResourceQuantity(_connectedResourceSO);
        }
    }
}