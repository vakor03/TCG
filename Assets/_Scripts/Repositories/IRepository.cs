using _Scripts.Managers;
using Zenject;

namespace _Scripts.Repositories
{
    public interface IRepository : IInitializable, IRequireSaving
    {
    }
}