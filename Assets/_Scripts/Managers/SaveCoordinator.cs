using Zenject;

namespace _Scripts.Managers
{
    public class SaveCoordinator
    {
        private readonly DiContainer _diContainer;

        public SaveCoordinator(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public void SaveAll()
        {
            var requireSaves = _diContainer
                .ResolveAll<IRequireSaving>();
            
            foreach (var requireSave in requireSaves)
            {
                requireSave.Save();
            }
        }
    }
}