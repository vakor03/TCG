using _Scripts.Factories;
using _Scripts.Interactors;
using _Scripts.Managers;
using _Scripts.Managers.Shops;
using _Scripts.Repositories;
using Zenject;

namespace _Scripts.DI.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindResourcesRepository();
            BindResourcesInteractor();

            BindLastTimeOnlineRepository();
            BindLastTimeOnlineInteractor();

            BindManagers();

            BindFactories(); 

            BindDatabases();

            BindShop();
        }

        private void BindDatabases()
        {
            BindProductionDatabase();
            BindMarketItemDatabase();
        }

        private void BindFactories()
        {
            BindProductionFactory();
        }

        private void BindManagers()
        {
            BindShopOptionManager();

            BindOfflineIncomeManager();

            BindSaveCoordinator();
        }

        private void BindLastTimeOnlineInteractor()
        {
            Container
                .Bind<LastTimeOnlineInteractor>()
                .AsSingle();
        }

        private void BindLastTimeOnlineRepository()
        {
            Container
                .BindInterfacesAndSelfTo<LastTimeOnlineRepository>()
                .AsSingle();
        }

        private void BindSaveCoordinator()
        {
            Container
                .Bind<SaveCoordinator>()
                .AsSingle();
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

        private void BindProductionDatabase()
        {
            Container
                .BindInterfacesAndSelfTo<ProductionDatabase>()
                .AsSingle().NonLazy();
        }

        private void BindShopOptionManager()
        {
            Container.Bind<ShopOptionManager>()
                .AsSingle();
        }

        private void BindOfflineIncomeManager()
        {
            Container.BindInterfacesAndSelfTo<OfflineIncomeManager>()
                .AsSingle();
        }
    }
}