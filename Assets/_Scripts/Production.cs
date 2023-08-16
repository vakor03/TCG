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

        // private float _productionRate;
        private ProductionSO _productionSO;
        private int _productionRateModifier = 1;
        private int _productionCountModifier = 1;

        public Production(ProductionSO productionSO)
        {
            _currentState = State.Stopped;
            _productionSO = productionSO;
        }

        public bool AutoProduction { get; private set; }

        public BigInteger ProductionCount
        {
            get => _productionCount;
            private set
            {
                _productionCount = value;
                OnProductionCountChanged?.Invoke();
            }
        }

        public float ProductionRate
        {
            get => _productionSO.BaseProductionRate / ProductionRateModifier;
            // private set
            // {
            //     _productionRate = value;
            //     OnProductionRateChanged?.Invoke();
            // }
        }

        public int ProductionCountModifier
        {
            get => _productionCountModifier;
            set
            {
                _productionCountModifier = value; 
                UpdateProductionCount();
            }
        }

        public int ProductionRateModifier
        {
            get => _productionRateModifier;
            set
            {
                _productionRateModifier = value;
                OnProductionRateChanged?.Invoke();
            }
        }

        public event Action OnProductionFinished;

        public event Action OnProductionStarted;

        public event Action OnProductionCountChanged;

        public event Action OnProductionRateChanged;
        
        public void OnStart()
        {
            ResourcesRepository.Instance.GetResource(_productionSO.ConnectedResource).OnCountChanged +=
                UpdateProductionCount;

            // ProductionRate = _productionSO.BaseProductionRate;
            UpdateProductionCount();
        }

        public void Dispose()
        {
            ResourcesRepository.Instance.GetResource(_productionSO.ConnectedResource).OnCountChanged -=
                UpdateProductionCount;
        }

        private void UpdateProductionCount()
        {
            ProductionCount = ResourcesRepository.Instance.GetResource(_productionSO.ConnectedResource).Count *
                              _productionSO.ProductionCount * ProductionCountModifier;
        }

        public void ToggleAutoProduction()
        {
            AutoProduction = !AutoProduction;

            if (AutoProduction)
            {
                if (_currentState == State.Stopped)
                {
                    StartOneTimeProductionCoroutine();
                }

                Timing.RunCoroutine(AutoProductionCoroutine());
            }
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

            yield return Timing.WaitForSeconds(ProductionRate);

            AddResourceToRepository();

            OnProductionFinished?.Invoke();
            _currentState = State.Stopped;
        }

        private IEnumerator<float> AutoProductionCoroutine()
        {
            while (AutoProduction)
            {
                yield return Timing.WaitUntilDone(_currentProductionCoroutine);
                StartOneTimeProductionCoroutine();
            }
        }

        private void AddResourceToRepository()
        {
            var productionResource = _productionSO.ProductionResource;

            ResourcesRepository.Instance.IncreaseResourceCount(productionResource, ProductionCount);
        }

        private enum State
        {
            Stopped,
            Production,
            AutoProduction
        }
    }
}