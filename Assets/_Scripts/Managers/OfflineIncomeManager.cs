﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using _Scripts.Helpers;
using _Scripts.Interactors;
using _Scripts.Repositories;
using _Scripts.ScriptableObjects;
using Zenject;

#endregion

namespace _Scripts.Managers
{
    public class OfflineIncomeManager
    {
        private LastTimeOnlineInteractor _lastTimeOnlineInteractor;
        private ProductionDatabase _productionDatabase;
        private ResourcesInteractor _resourcesInteractor;

        private OfflineIncomeManager(ResourcesInteractor resourcesInteractor,
            ProductionDatabase productionDatabase,
            LastTimeOnlineInteractor lastTimeOnlineInteractor)
        {
            _resourcesInteractor = resourcesInteractor;
            _productionDatabase = productionDatabase;
            _lastTimeOnlineInteractor = lastTimeOnlineInteractor;
        }

        public Dictionary<ResourceSO, BigInteger> OfflineIncome { get; private set; }

        public void Setup()
        {
            OfflineIncome = CalculateOfflineIncome();
        }

        private Dictionary<ResourceSO, BigInteger> CalculateOfflineIncome()
        {
            var income = new Dictionary<ResourceSO, BigInteger>();

            foreach (var productionSO in _productionDatabase.ProductionSOs)
            {
                var productionStats = _productionDatabase.GetProductionStats(productionSO);
                var productionResource = productionSO.ProductionResource;

                var connectedResourceQuantity =
                    _resourcesInteractor.GetResourceQuantity(productionSO.ConnectedResource);
                var productionSpeed = productionStats.GetProductionSpeed();
                var secondsSinceOnline = _lastTimeOnlineInteractor.GetTimeFromSinceTimeOnline().ToTotalSeconds();
                
                var totalSeconds = new BigInteger(secondsSinceOnline);

                var producedQuantity = connectedResourceQuantity
                                       * productionSpeed
                                       * totalSeconds;

                income.Add(productionResource, producedQuantity);
            }

            return income;
        }

        public void ReceiveIncome()
        {
            _resourcesInteractor.AddResources(OfflineIncome);
        }

        public List<ProductionSO> GetFinalProductions()
        {
            var finalProductions = new List<ProductionSO>();
            foreach (var productionSO in _productionDatabase.ProductionSOs)
            {
                if (_productionDatabase.ProductionSOs.All(so =>
                        so.ProductionResource != productionSO.ConnectedResource))
                {
                    finalProductions.Add(productionSO);
                }
            }

            return finalProductions;
        }

        public TimeSpan GetTimeSinceLastOnline()
        {
           return _lastTimeOnlineInteractor.GetTimeFromSinceTimeOnline();
        }
    }
}