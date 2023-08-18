namespace _Scripts.Repositories
{
    public static class RepositoriesHelper
    {
        public static T GetRepository<T>() where T : IRepository
        {
            return GameManager.Instance.RepositoriesBase.GetRepository<T>();
        }
    }
}