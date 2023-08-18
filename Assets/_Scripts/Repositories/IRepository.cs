namespace _Scripts.Repositories
{
    public interface IRepository
    {
        void Initialize();

        void OnStart()
        {
        }

        void Save();
    }
}