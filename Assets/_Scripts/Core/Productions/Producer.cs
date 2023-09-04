#region

using System;
using System.Collections.Generic;
using _Scripts.Core.Productions.Upgrades;
using _Scripts.Interactors;
using _Scripts.ScriptableObjects;
using UnityEngine;

#endregion

namespace _Scripts.Core.Productions
{
    public class Producer : IProducer
    {
        public ProductionStats BaseStats { get; }
        public ProductionStats CurrentStats { get; private set; }
        
        private Production _production;
        private List<IUpgrade> _upgrades = new();
        
        public Producer(ProductionStats baseStats,
            ResourceSO productionResourceSO,
            ResourceSO connectedResource,
            ResourcesInteractor resourcesInteractor)
        {
            BaseStats = baseStats;
            CurrentStats = BaseStats;

            _production = new Production(this, productionResourceSO, connectedResource, resourcesInteractor);
            _production.OnStarted += () => { OnProductionStarted?.Invoke(); };
            _production.OnFinished += () => { OnProductionFinished?.Invoke(); };
        }

        public void AddUpgrade(IUpgrade upgrade)
        {
            _upgrades.Add(upgrade);

            UpdateProductionStats();

            OnStatsChanged?.Invoke();
            LogStatsUpdated();
        }

        public void RemoveUpgrade(IUpgrade upgrade)
        {
            _upgrades.Remove(upgrade);

            UpdateProductionStats();

            OnStatsChanged?.Invoke();
            LogStatsUpdated();
        }

        public event Action OnStatsChanged;
        public void StartProduction()
        {
            _production.StartProduction();
        }

        public event Action OnProductionFinished;
        public event Action OnProductionStarted;

        private void UpdateProductionStats()
        {
            CurrentStats = BaseStats;

            foreach (var upgrade in _upgrades)
            {
                CurrentStats = upgrade.ApplyUpgrade(CurrentStats);
            }
        }

        private void LogStatsUpdated()
        {
            Debug.Log("Stats updated");
            Debug.Log(CurrentStats.ToString());
        }
    }
}