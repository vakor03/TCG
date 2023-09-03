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
        private InteractorsBase InteractorsBase { get; set; }
        private RepositoriesBase RepositoriesBase { get; set; }

        protected override void Awake()
        {
            base.Awake();

            RepositoriesBase.InitializeAllRepositories();
            InteractorsBase.InitializeAllInteractors();

            RepositoriesBase.SendOnStartToAllRepositories();
        }

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
                RepositoriesBase.SaveAllRepositories();
            }
        }

        protected override void OnApplicationQuit()
        {
            RepositoriesBase.SaveAllRepositories();
        }

        [Inject]
        public void Construct(RepositoriesBase repositoriesBase,
            InteractorsBase interactorsBase,
            OfflineIncomeManager offlineIncomeManager,
            IProductionUIFactory productionUIFactory)
        {
            RepositoriesBase = repositoriesBase;
            InteractorsBase = interactorsBase;
            _offlineIncomeManager = offlineIncomeManager;
            _productionUIFactory = productionUIFactory;
        }
    }
}