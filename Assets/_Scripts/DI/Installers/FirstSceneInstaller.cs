using _Scripts.Core.Productions;
using _Scripts.Factories;
using UnityEngine;
using Zenject;

namespace _Scripts.DI.Installers
{
    public class FirstSceneInstaller : MonoInstaller
    {
        [SerializeField] private ProductionUIContainerMB productionUIContainerMB;

        public override void InstallBindings()
        {
            BindProductionUIFactory();
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
    }
}