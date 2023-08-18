#region

using System;
using System.Collections.Generic;
using System.Numerics;
using _Scripts.Interactors;
using _Scripts.Repositories;
using _Scripts.ScriptableObjects;
using MEC;

#endregion

namespace _Scripts
{
    public class Production : IDisposable
    {
        private CoroutineHandle _currentProductionCoroutine;
        private State _currentState;

        private BigInteger _productionCount;

        private ProductionStats _productionStats;
        private ResourceSO _productedResource;
        private ResourceSO _connectedResource;

        public Production(ProductionStats productionStats, ResourceSO productedResource, ResourceSO connectedResource)
        {
            _currentState = State.Stopped;
            _productionStats = productionStats;
            _productedResource = productedResource;
            _connectedResource = connectedResource;

            _productionStats.OnAutoProductionChanged += ProductionStatsOnAutoProductionChanged;
            _productionStats.OnProductionRateChanged += () => OnProductionRateChanged?.Invoke();
            _productionStats.OnProductionCountChanged += UpdateProductionCount;
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

        public event Action OnProductionFinished;

        public event Action OnProductionStarted;

        public event Action OnProductionCountChanged;

        public event Action OnProductionRateChanged;

        public void OnStart()
        {
            InteractorsHelper.GetInteractor<ResourcesInteractor>()
                .OnResourceQuantityChanged += ResourcesRepositoryOnResourceQuantityChanged;

            // ProductionRate = _productionSO.BaseProductionRate;
            UpdateProductionCount();
        }

        private void ResourcesRepositoryOnResourceQuantityChanged(ResourceSO changedResource)
        {
            if (changedResource == _connectedResource)
            {
                UpdateProductionCount();
            }
        }

        public void Dispose()
        {
            InteractorsHelper.GetInteractor<ResourcesInteractor>().OnResourceQuantityChanged -= ResourcesRepositoryOnResourceQuantityChanged;
        }

        private void UpdateProductionCount()
        {
            ProductionCount = InteractorsHelper.GetInteractor<ResourcesInteractor>().GetResourceQuantity(_connectedResource) *
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

            InteractorsHelper.GetInteractor<ResourcesInteractor>().AddResource(_productedResource, ProductionCount);

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