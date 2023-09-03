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
            BindResourcesRepository();

            BindResourcesInteractor();

            BindOfflineIncomeManager();

            BindShopOptionManager();

            BindProductionUIFactory();

            BindProductionFactory();

            BindProductionContainer();

            BindShop();
            
            BindMarketItemDatabase();
        }

        private void BindResourcesInteractor()
        {
            Container
                .Bind<ResourcesInteractor>()
                .AsSingle();
        }

        private void BindResourcesRepository()
        {
            Container
                .BindInterfacesAndSelfTo<ResourcesRepository>()
                .AsSingle();
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
    }
}