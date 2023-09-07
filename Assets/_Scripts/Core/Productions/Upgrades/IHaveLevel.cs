namespace _Scripts.Core.Productions.Upgrades
{
    public interface IHaveLevel
    {
        int Level { get; }
        void ChangeLevel(int newValue);
    }
}