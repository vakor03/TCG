using System;
using _Scripts.Helpers;
using _Scripts.Interactors;
using _Scripts.Repositories;
using _Scripts.ScriptableObjects;
using _Scripts.UI;
using UnityEngine;
using Zenject;

namespace _Scripts
{
    public class GameManager : StaticInstance<GameManager>
    {
        [Inject] public InteractorsBase InteractorsBase { get; private set; }
        [Inject] public RepositoriesBase RepositoriesBase { get; private set; }
        [Inject] private OfflineIncomeManager _offlineIncomeManager;

        [SerializeField] private LevelConfigSO firstLevelConfigSO;

        protected override void Awake()
        {
            base.Awake();

            // RepositoriesBase = new RepositoriesBase();
            // InteractorsBase = new InteractorsBase();
            //
            // RepositoriesBase.CreateAllRepositories();
            // InteractorsBase.CreateAllInteractors();
            //
            // RepositoriesBase.InitializeAllRepositories();
            // InteractorsBase.InitializeAllInteractors();
            //
            // RepositoriesBase.SendOnStartToAllRepositories();

            Debug.Assert(RepositoriesBase != null, nameof(RepositoriesBase) + " != null");
            Debug.Assert(InteractorsBase != null, nameof(InteractorsBase) + " != null");

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