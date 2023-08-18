using System;
using _Scripts.Helpers;
using _Scripts.Interactors;
using _Scripts.Repositories;
using UnityEngine;

namespace _Scripts
{
    public class GameManager : StaticInstance<GameManager>
    {
        public InteractorsBase InteractorsBase { get; private set; }
        public RepositoriesBase RepositoriesBase { get; private set; }
        
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
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                RepositoriesBase.SaveAllRepositories();
            }
        }

        private void OnApplicationQuit()
        {
            RepositoriesBase.SaveAllRepositories();
        }
    }
}