namespace _Scripts.Interactors
{
    public static class InteractorsHelper
    {
        public static T GetInteractor<T>() where T : IInteractor
        {
            return GameManager.Instance.InteractorsBase.GetInteractor<T>();
        }
    }
}