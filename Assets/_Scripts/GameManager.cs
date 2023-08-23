using System;
using _Scripts.Helpers;
using _Scripts.Interactors;
using _Scripts.Repositories;
using _Scripts.ScriptableObjects;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts
{
    public class GameManager : StaticInstance<GameManager>
    {
        public InteractorsBase InteractorsBase { get; private set; }
        public RepositoriesBase RepositoriesBase { get; private set; }
        [SerializeField] private LevelConfigSO firstLevelConfigSO;
        private OfflineIncomeManager _offlineIncomeManager;

        protected override void Awake()
        {
            base.Awake();

            RepositoriesBase = new RepositoriesBase();
            InteractorsBase = new InteractorsBase();

            RepositoriesBase.CreateAllRepositories();
            InteractorsBase.CreateAllInteractors();

            RepositoriesBase.InitializeAllRepositories();
            InteractorsBase.InitializeAllInteractors();

            RepositoriesBase.SendOnStartToAllRepositories();

            _offlineIncomeManager = new OfflineIncomeManager();
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

            foreach (var production in firstLevelConfigSO.ProductionsAvailable)
            {
                ProductionsGroup.Instance.AddProduction(production);
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

        private void OnApplicationQuit()
        {
            RepositoriesBase.SaveAllRepositories();
        }
    }
}