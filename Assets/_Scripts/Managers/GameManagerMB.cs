#region

using _Scripts.Factories;
using _Scripts.Helpers;
using _Scripts.Interactors;
using _Scripts.Repositories;
using _Scripts.ScriptableObjects;
using _Scripts.UI;
using UnityEngine;
using Zenject;

#endregion

namespace _Scripts.Managers
{
    public class GameManagerMB : StaticInstance<GameManagerMB>
    {
        [SerializeField] private LevelConfigSO firstLevelConfigSO;
        private OfflineIncomeManager _offlineIncomeManager;

        private IProductionUIFactory _productionUIFactory;
        private ResourcesRepository _resourcesRepository;

        private void Start()
        {
            if (_offlineIncomeManager.TryGetTimeFromLastTimeOnline(out var seconds, out var time))
            {
                OfflineIncomeUI.Instance.Show();
                OfflineIncomeUI.Instance.Setup(time, seconds, _offlineIncomeManager);
            }
            else
            {
                OfflineIncomeUI.Instance.Hide();
                Debug.Log("First time playing?");
            }

            foreach (var productionSO in firstLevelConfigSO.ProductionsAvailable)
            {
                _productionUIFactory.Create(productionSO);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                _offlineIncomeManager.Save();
                _resourcesRepository.Save();
            }
        }

        protected override void OnApplicationQuit()
        {
            _resourcesRepository.Save();
        }

        [Inject]
        public void Construct(ResourcesRepository resourcesRepository,
            OfflineIncomeManager offlineIncomeManager,
            IProductionUIFactory productionUIFactory)
        {
            _resourcesRepository = resourcesRepository;
            _offlineIncomeManager = offlineIncomeManager;
            _productionUIFactory = productionUIFactory;
        }
    }
}