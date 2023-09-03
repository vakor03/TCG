using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using _Scripts.Repositories;
using _Scripts.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace _Scripts
{
    public class OfflineIncomeManager
    {
        private readonly string _dateTimeFormat = "u";
        private readonly CultureInfo _dateTimeCulture = CultureInfo.InvariantCulture;
        private const string LAST_TIME_ONLINE_KEY = "LAST_TIME_OFFLINE_KEY";
        private const float MAX_SECONDS_OFFLINE_COUNT = float.MaxValue;

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

        private ResourcesRepository _resourcesRepository;
        private ProductionContainer _productionContainer;
        
        private OfflineIncomeManager(RepositoriesBase repositoriesBase,
            ProductionContainer productionContainer)
        {
            _resourcesRepository = repositoriesBase.GetRepository<ResourcesRepository>();
            _productionContainer = productionContainer;
        }


        public Dictionary<ResourceSO, BigInteger> CalculateOfflineIncome(float seconds)
        {
            var income = new Dictionary<ResourceSO, BigInteger>();

            foreach (var productionSO in _productionContainer.ProductionSOs)
            {
                var productionStats = _productionContainer.GetProductionStats(productionSO);
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
            foreach (var productionSO in _productionContainer.ProductionSOs)
            {
                if (_productionContainer.ProductionSOs.All(so =>
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