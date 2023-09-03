using _Scripts.Core.Productions;
using _Scripts.Factories;
using _Scripts.Interactors;
using _Scripts.Managers;
using _Scripts.Repositories;
using UnityEngine;
using Zenject;

namespace _Scripts.DI.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private ProductionUIContainerMB productionUIContainerMB;

        public override void InstallBindings()
        {
            BindRepositoriesBaseAndInteractorsBase();

            BindOfflineIncomeManager();

            BindShopOptionManager();

            BindProductionUIFactory();

            BindProductionFactory();

            BindProductionContainer();

            BindShop();
            
            BindMarketItemDatabase();
        }

        private void BindMarketItemDatabase()
        {
            Container
                .BindInterfacesAndSelfTo<MarketItemDatabase>()
                .AsSingle();
        }

        private void BindShop()
        {
            Container.Bind<Shop>().AsSingle();
        }

        private void BindProductionFactory()
        {
            Container.Bind<IProductionFactory>()
                .To<ProductionFactory>()
                .AsSingle();
        }

        private void BindProductionUIFactory()
        {
            Container
                .BindInstance(productionUIContainerMB)
                .AsSingle();

            Container
                .Bind<IProductionUIFactory>()
                .To<ProductionUIFactory>()
                .AsSingle();
        }

        private void BindProductionContainer()
        {
            Container
                .BindInterfacesAndSelfTo<ProductionDatabase>()
                .AsSingle();
        }

        private void BindShopOptionManager()
        {
            Container.Bind<ShopOptionManager>()
                .AsSingle();
        }

        private void BindOfflineIncomeManager()
        {
            Container.Bind<OfflineIncomeManager>()
                .AsSingle();
        }

        private void BindRepositoriesBaseAndInteractorsBase()
        {
            RepositoriesBase repositoriesBase = new RepositoriesBase();
            InteractorsBase interactorsBase = new InteractorsBase(repositoriesBase);

            repositoriesBase.CreateAllRepositories();
            interactorsBase.CreateAllInteractors();

            Container
                .Bind<RepositoriesBase>()
                .FromInstance(repositoriesBase)
                .AsSingle();

            Container
                .Bind<InteractorsBase>()
                .FromInstance(interactorsBase)
                .AsSingle();

            Container.Bind<ResourcesInteractor>()
                .FromInstance(interactorsBase.GetInteractor<ResourcesInteractor>())
                .AsSingle();
        }
    }
}