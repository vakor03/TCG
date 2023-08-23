namespace _Scripts.Repositories
{
    public interface IRepository
    {
        public void Initialize();

        public void OnStart()
        {
        }

        public void Save();
    }
}