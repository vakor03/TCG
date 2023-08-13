using System;
using UnityEngine;

namespace _Scripts
{
    public class ProducerMB : MonoBehaviour
    {
        [SerializeField] private float productionRate;
        [SerializeField] private int productionCount;

        public float GetProgressNormalized()
        {
            return _currentTime / productionRate;
        }

        private float _currentTime;
        private void Update()
        {
            _currentTime += Time.deltaTime;
            if (_currentTime > productionRate)
            {
                _currentTime = 0;
                Money.Instance.IncreaseValue(productionCount);
                Debug.Log("Production");
            }
        }
    }
}