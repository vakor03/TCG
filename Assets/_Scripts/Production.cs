#region

using System;
using System.Collections.Generic;
using System.Numerics;
using _Scripts.Repositories;
using _Scripts.ScriptableObjects;
using _Scripts.Upgrades;
using MEC;

#endregion

namespace _Scripts
{
    public class Production : IDisposable
    {
        private CoroutineHandle _currentProductionCoroutine;
        private State _currentState;

        private BigInteger _productionCount;

        // private ProductionSO _productionSO;
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
            ResourcesRepository.Instance.GetResource(_connectedResource).OnCountChanged +=
                UpdateProductionCount;

            // ProductionRate = _productionSO.BaseProductionRate;
            UpdateProductionCount();
        }

        public void Dispose()
        {
            ResourcesRepository.Instance.GetResource(_connectedResource).OnCountChanged -=
                UpdateProductionCount;
        }

        private void UpdateProductionCount()
        {
            ProductionCount = ResourcesRepository.Instance.GetResource(_connectedResource).Count *
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

            ResourcesRepository.Instance.IncreaseResourceCount(_productedResource, ProductionCount);

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

    public class ProductionStats
    {
        public bool AutoProduction { get; private set; }
        public BigInteger ProductionCount { get; private set; }
        public float ProductionRate { get; private set; }
        private List<ProductionUpgrade> _productionUpgrades;

        public event Action<bool> OnAutoProductionChanged;
        public event Action OnProductionCountChanged;
        public event Action OnProductionRateChanged;

        public ProductionStats(ProductionSO productionSO)
        {
            AutoProduction = false;
            ProductionCount = productionSO.ProductionCount;
            ProductionRate = productionSO.BaseProductionRate;
        }

        public void UnlockUpgrade(ProductionUpgrade productionUpgrade)
        {
            if (productionUpgrade.UpgradeType == UpgradeType.AutoProduction)
            {
                AutoProduction = true;
                OnAutoProductionChanged?.Invoke(true);
            }
            else if (productionUpgrade.UpgradeType == UpgradeType.ProductionRate)
            {
                var productionRateUpgrade = (ProductionRateUpgrade) productionUpgrade;
                ProductionRate /= productionRateUpgrade.Value;
                OnProductionRateChanged?.Invoke();
            }
        }
    }
}