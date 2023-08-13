using System.Numerics;
using _Scripts.Enums;
using _Scripts.ScriptableObjects;

namespace _Scripts
{
    public class Resource
    {
        public ResourceType Type => _resourceSO.Type;
        private BigInteger _count;
        private ResourceSO _resourceSO;

        public float ProductionRate { get; private set; }
        public BigInteger ProductionCount { get; private set; }
        public ResourceSO SO => _resourceSO;
        public BigInteger Count => _count;

        public ResourceProducer Producer { get; private set; }

        public Resource(ResourceSO resourceSO)
        {
            _resourceSO = resourceSO;
            Producer = new ResourceProducer(this, _resourceSO.ProductionResourceType);
            
            Init();
        }

        public void Init()
        {
            ProductionRate = 2;
            ProductionCount = 3;
            _count = 1;
        }

        public void IncreaseCount(BigInteger value)
        {
            _count += value;
        }
    }
}