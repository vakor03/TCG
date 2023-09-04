﻿using _Scripts.Core.Productions;
using _Scripts.ScriptableObjects;

namespace _Scripts.Factories
{
    public interface IProductionFactory
    {
        Producer Create(ProductionStats productionStats,
            ResourceSO productedResource,
            ResourceSO connectedResource);
    }
}