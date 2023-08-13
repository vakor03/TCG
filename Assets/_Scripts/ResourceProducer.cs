using System;
using System.Collections.Generic;
using _Scripts.Enums;
using _Scripts.Managers;
using MEC;
using UnityEngine;

namespace _Scripts
{
    public class ResourceProducer
    {
        public event Action OnProductionFinished;
        public event Action OnProductionStarted;

        public bool AutoProduction { get; private set; }

        private State _currentState;
        private Resource _resource;
        private ResourceType _productionResource;

        public ResourceProducer(Resource resource, ResourceType productionResource)
        {
            _resource = resource;
            _currentState = State.Stopped;
            _productionResource = productionResource;
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

            Debug.Log(_resource.ProductionRate);
            yield return Timing.WaitForSeconds(_resource.ProductionRate);

            AddResourceToRepository();
            OnProductionFinished?.Invoke();
            _currentState = State.Stopped;
        }

        private void AddResourceToRepository()
        {
            var productionCount = _resource.ProductionCount * _resource.Count;
            ResourcesRepository.Instance.IncreaseResourceCount(_productionResource, productionCount);
        }

        private enum State
        {
            Stopped,
            Production
        }
    }
}