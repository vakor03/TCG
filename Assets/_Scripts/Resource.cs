using System;
using System.Numerics;
using _Scripts.ScriptableObjects;

namespace _Scripts
{
   public class Resource
    {
        private ResourceSO _resourceSO;
        private BigInteger _count;

        public event Action OnCountChanged;

        public ResourceSO ResourceSO
        {
            get => _resourceSO;
        }

        public BigInteger Count
        {
            get => _count;
            set
            {
                _count = value;
                OnCountChanged?.Invoke();
            }
        }

        public Resource(ResourceSO resourceSO)
        {
            _resourceSO = resourceSO;
        }
    }
}