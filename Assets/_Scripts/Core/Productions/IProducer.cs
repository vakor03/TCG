using System;

namespace _Scripts.Core.Productions
{
    public interface IProducer : IUpgradable
    {
        event Action OnStatsChanged;
        void StartProduction();
        ProductionStats BaseStats { get; }
        ProductionStats CurrentStats { get; }
    }
}