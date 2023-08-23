using _Scripts.Interactors;
using _Scripts.Repositories;
using Zenject;

namespace _Scripts
{
    public class GameInstaller : MonoInstaller
    {
        public RepositoriesBase RepositoriesBase;
        public InteractorsBase InteractorsBase;
        public override void InstallBindings()
        {
            RepositoriesBase = new RepositoriesBase();
            InteractorsBase = new InteractorsBase();
            
            RepositoriesBase.CreateAllRepositories();
            InteractorsBase.CreateAllInteractors();
            
            InstallRepositoriesBindings();

            Container.Bind<RepositoriesBase>()
                .FromInstance(RepositoriesBase)
                .AsSingle();

            Container.Bind<InteractorsBase>()
                .FromInstance(InteractorsBase)
                .AsSingle();
            
            Container.Bind<OfflineIncomeManager>()
                .AsSingle();
        }

        private void InstallRepositoriesBindings()
        {
            Container.Bind<IResourcesRepository>()
                .To<ResourcesRepository>()
                .FromInstance(RepositoriesBase.GetRepository<ResourcesRepository>())
                .AsSingle()
                .NonLazy();

            Container.Bind<IProductionsRepository>()
                .To<ProductionsRepository>()
                .FromInstance(RepositoriesBase.GetRepository<ProductionsRepository>())
                .AsSingle().NonLazy();
        }
    }
}