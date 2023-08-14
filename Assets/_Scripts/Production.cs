using System;
using System.Collections.Generic;
using System.Numerics;
using _Scripts.Repositories;
using _Scripts.ScriptableObjects;
using MEC;

namespace _Scripts
{
    public class Production : IDisposable
    {
        public event Action OnProductionFinished;
        public event Action OnProductionStarted;
        public event Action OnProductionCountChanged;
        public event Action OnProductionRateChanged;

        public bool AutoProduction { get; private set; }

        private State _currentState;
        private ProductionSO _productionSO;

        public BigInteger ProductionCount
        {
            get => _productionCount;
            set
            {
                _productionCount = value;
                OnProductionCountChanged?.Invoke();
            }
        }

        private BigInteger _productionCount;

        private float _productionRate;

        public float ProductionRate
        {
            get => _productionRate;
            set
            {
                _productionRate = value;
                OnProductionRateChanged?.Invoke();
            }
        }

        public Production(ProductionSO productionSO)
        {
            _currentState = State.Stopped;
            _productionSO = productionSO;
        }

        public void OnStart()
        {
            ResourcesRepository.Instance.GetResource(_productionSO.ConnectedResource).OnCountChanged +=
                CalculateNewProductionCount;

            ProductionRate = _productionSO.ProductionRate;
            CalculateNewProductionCount();
        }

        private void CalculateNewProductionCount()
        {
            ProductionCount = ResourcesRepository.Instance.GetResource(_productionSO.ConnectedResource).Count *
                              _productionSO.ProductionCount;
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

        private CoroutineHandle _currentProductionCoroutine;

        private void StartOneTimeProductionCoroutine()
        {
            _currentProductionCoroutine = Timing.RunCoroutine(OneTimeProductionCoroutine());
        }

        private IEnumerator<float> OneTimeProductionCoroutine()
        {
            _currentState = State.Production;
            OnProductionStarted?.Invoke();

            yield return Timing.WaitForSeconds(_productionSO.ProductionRate);

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

        public void Dispose()
        {
            ResourcesRepository.Instance.GetResource(_productionSO.ConnectedResource).OnCountChanged -=
                CalculateNewProductionCount;
        }
    }
}