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
    public class Production : IDisposable
    {
        private ResourceSO _connectedResource;
        private CoroutineHandle _currentProductionCoroutine;
        private State _currentState;
        private ResourceSO _productedResource;

        private BigInteger _productionCount;

        private ProductionStats _productionStats;
        public ResourcesInteractor _resourcesInteractor;

        
        public Production(ProductionStats productionStats, 
            ResourceSO productedResource, 
            ResourceSO connectedResource,
            ResourcesInteractor resourcesInteractor)
        {
            _currentState = State.Stopped;
            _productionStats = productionStats;
            _productedResource = productedResource;
            _connectedResource = connectedResource;

            _productionStats.OnAutoProductionChanged += ProductionStatsOnAutoProductionChanged;
            _productionStats.OnProductionRateChanged += () => OnProductionRateChanged?.Invoke();
            _productionStats.OnProductionCountChanged += UpdateProductionCount;
            
            _resourcesInteractor = resourcesInteractor;
            
            OnStart();
        }

        public BigInteger ProductionCount
        {
            get => _productionCount;
            private set
            {
                _productionCount = value;
                OnProductionCountChanged?.Invoke();
            }
        }

        public float ProductionRate => _productionStats.ProductionRate;

        public void Dispose()
        {
            _resourcesInteractor.OnResourceQuantityChanged -= ResourcesRepositoryOnResourceQuantityChanged;
        }

        private void ProductionStatsOnAutoProductionChanged(bool value)
        {
            if (value)
            {
                if (_currentState == State.Stopped)
                {
                    StartOneTimeProductionCoroutine();
                }

                Timing.RunCoroutine(AutoProductionCoroutine());
            }
        }

        public event Action OnProductionFinished;

        public event Action OnProductionStarted;

        public event Action OnProductionCountChanged;

        public event Action OnProductionRateChanged;

        public void OnStart()
        {
            _resourcesInteractor
                .OnResourceQuantityChanged += ResourcesRepositoryOnResourceQuantityChanged;

            UpdateProductionCount();
        }


        private void ResourcesRepositoryOnResourceQuantityChanged(ResourceSO changedResource)
        {
            if (changedResource == _connectedResource)
            {
                UpdateProductionCount();
            }
        }

        private void UpdateProductionCount()
        {
            ProductionCount = _resourcesInteractor.GetResourceQuantity(_connectedResource) *
                              _productionStats.ProductionCount;
        }

        public void StartProduction()
        {
            if (_currentState == State.Production)
            {
                return;
            }

            StartOneTimeProductionCoroutine();
        }

        private void StartOneTimeProductionCoroutine()
        {
            _currentProductionCoroutine = Timing.RunCoroutine(OneTimeProductionCoroutine());
        }

        private IEnumerator<float> OneTimeProductionCoroutine()
        {
            _currentState = State.Production;
            OnProductionStarted?.Invoke();

            yield return Timing.WaitForSeconds(_productionStats.ProductionRate);

            _resourcesInteractor.AddResource(_productedResource, ProductionCount);

            OnProductionFinished?.Invoke();
            _currentState = State.Stopped;
        }

        private IEnumerator<float> AutoProductionCoroutine()
        {
            while (_productionStats.AutoProduction)
            {
                yield return Timing.WaitUntilDone(_currentProductionCoroutine);
                StartOneTimeProductionCoroutine();
            }
        }

        private enum State
        {
            Stopped,
            Production,
            AutoProduction
        }
    }
}