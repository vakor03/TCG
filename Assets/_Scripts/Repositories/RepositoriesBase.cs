using System;
using System.Collections.Generic;

namespace _Scripts.Repositories
{
    public class RepositoriesBase
    {
        private Dictionary<Type, IRepository> _repositoriesMap = new();
        
        public void CreateAllRepositories()
        {
            CreateRepository<ResourcesRepository>();
            CreateRepository<ProductionsRepository>();
            CreateRepository<MarketRepository>();
        }
        
        private void CreateRepository<T>() where T : IRepository, new()
        {
            var repository = new T();
            _repositoriesMap.Add(typeof(T), repository);
        }
        
        public T GetRepository<T>() where T : IRepository
        {
            return (T)_repositoriesMap[typeof(T)];
        }
        
        public void InitializeAllRepositories()
        {
            foreach (var repository in _repositoriesMap.Values)
            {
                repository.Initialize();
            }
        }
        
        public void SendOnStartToAllRepositories()
        {
            foreach (var repository in _repositoriesMap.Values)
            {
                repository.OnStart();
            }
        }
        
        public void SaveAllRepositories()
        {
            foreach (var repository in _repositoriesMap.Values)
            {
                repository.Save();
            }
        }
    }
}