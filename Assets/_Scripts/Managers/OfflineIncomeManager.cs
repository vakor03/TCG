#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using _Scripts.Repositories;
using _Scripts.ScriptableObjects;
using UnityEngine;

#endregion

namespace _Scripts.Managers
{
    public class OfflineIncomeManager
    {
        private const string LAST_TIME_ONLINE_KEY = "LAST_TIME_OFFLINE_KEY";
        private const float MAX_SECONDS_OFFLINE_COUNT = float.MaxValue;
        private readonly CultureInfo _dateTimeCulture = CultureInfo.InvariantCulture;
        private readonly string _dateTimeFormat = "u";
        private ProductionDatabase _productionDatabase;

        private ResourcesRepository _resourcesRepository;

        private OfflineIncomeManager(ResourcesRepository resourcesRepository,
            ProductionDatabase productionDatabase)
        {
            _resourcesRepository = resourcesRepository;
            _productionDatabase = productionDatabase;
        }

        public event Action OnFirstGameEnter;

        public bool TryGetTimeFromLastTimeOnline(out float seconds, out TimeSpan difference)
        {
            if (PlayerPrefs.HasKey(LAST_TIME_ONLINE_KEY))
            {
                var timeNow = DateTime.UtcNow;
                var lastSaveTime = PlayerPrefs.GetString(LAST_TIME_ONLINE_KEY);
                var lastSaveDateTime = DateTime.ParseExact(lastSaveTime, _dateTimeFormat, _dateTimeCulture);
                difference = timeNow - lastSaveDateTime;
                seconds = Mathf.Clamp((float)(difference).TotalSeconds, 0f, MAX_SECONDS_OFFLINE_COUNT);
                return true;
            }
            else
            {
                seconds = 0;
                difference = new TimeSpan();
                OnFirstGameEnter?.Invoke();
                return false;
            }
        }


        public Dictionary<ResourceSO, BigInteger> CalculateOfflineIncome(float seconds)
        {
            var income = new Dictionary<ResourceSO, BigInteger>();

            foreach (var productionSO in _productionDatabase.ProductionSOs)
            {
                var productionStats = _productionDatabase.GetProductionStats(productionSO);
                var productionResource = productionSO.ProductionResource;

                var connectedResourceQuantity =
                    _resourcesRepository.GetResourceQuantity(productionSO.ConnectedResource);
                var productionSpeed = productionStats.GetProductionSpeed();
                var totalSeconds = new BigInteger(seconds);


                var producedQuantity = connectedResourceQuantity
                                       * productionSpeed
                                       * totalSeconds;

                income.Add(productionResource, producedQuantity);
            }

            return income;
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

        public void Save()
        {
            var timeNow = DateTime.UtcNow;
            string timeString = timeNow.ToString(_dateTimeFormat, _dateTimeCulture);
            PlayerPrefs.SetString(LAST_TIME_ONLINE_KEY, timeString);
            PlayerPrefs.Save();
        }
    }
}