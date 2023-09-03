using System;
using System.Collections.Generic;
using _Scripts.Repositories;

namespace _Scripts.Interactors
{
    public class InteractorsBase
    {
        private RepositoriesBase _repositoriesBase;
        private Dictionary<Type, IInteractor> _interactorsMap = new();
        
        public InteractorsBase(RepositoriesBase repositoriesBase)
        {
            _repositoriesBase = repositoriesBase;
        }

        public void CreateAllInteractors()
        {
            CreateInteractor<ResourcesInteractor>();
        }

        private void CreateInteractor<T>() where T : IInteractor, new()
        {
            var interactor = new T();
            _interactorsMap.Add(typeof(T), interactor);
        }

        public T GetInteractor<T>() where T : IInteractor
        {
            return (T)_interactorsMap[typeof(T)];
        }

        public void InitializeAllInteractors()
        {
            foreach (var interactor in _interactorsMap.Values)
            {
                interactor.Initialize(_repositoriesBase);
            }
        }
    }
}