#region

using _Scripts.Factories;
using _Scripts.Interactors;
using _Scripts.ScriptableObjects;
using _Scripts.UI;
using UnityEngine;
using Zenject;

#endregion

namespace _Scripts.Managers
{
    public class GameManagerMB : MonoBehaviour
    {
        [SerializeField] private LevelConfigSO firstLevelConfigSO;
        private OfflineIncomeManager _offlineIncomeManager;

        private IProductionUIFactory _productionUIFactory;
        private OfflineIncomeUI _offlineIncomeUI;
        private SaveCoordinator _saveCoordinator;
        private LastTimeOnlineInteractor _lastTimeOnlineInteractor;

        private void Start()
        {
            if (IsFirstTimePlaying())
            {
                StartAsFirstTimePlaying();
            }
            else
            {
                StartAsRegular();
            }

            CreateNecessaryProductionsUI();

            bool IsFirstTimePlaying()
            {
                return _lastTimeOnlineInteractor.IsFirstTimePlaying();
            }
        }

        private void StartAsRegular()
        {
            _offlineIncomeManager.Setup();
            _offlineIncomeUI.Show();
            _offlineIncomeUI.UpdateText();
        }

        private void StartAsFirstTimePlaying()
        {
            _offlineIncomeUI.Hide();
            Debug.Log("First time playing?");
        }

        private void CreateNecessaryProductionsUI()
        {
            foreach (var productionSO in firstLevelConfigSO.ProductionsAvailable)
            {
                _productionUIFactory.Create(productionSO);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                _saveCoordinator.SaveAll();
            }
        }

        [Inject]
        public void Construct(
            OfflineIncomeManager offlineIncomeManager,
            IProductionUIFactory productionUIFactory,
            OfflineIncomeUI offlineIncomeUI,
            SaveCoordinator saveCoordinator,
            LastTimeOnlineInteractor lastTimeOnlineInteractor)
        {
            _offlineIncomeManager = offlineIncomeManager;
            _productionUIFactory = productionUIFactory;
            _offlineIncomeUI = offlineIncomeUI;
            _saveCoordinator = saveCoordinator;
            _lastTimeOnlineInteractor = lastTimeOnlineInteractor;
        }
    }
}