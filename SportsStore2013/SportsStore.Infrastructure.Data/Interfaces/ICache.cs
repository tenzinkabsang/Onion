namespace SportsStore.Infrastructure.Data.Interfaces
{
    public interface ICache
    {
        T Get<T>(string key) where T : class;

        void Add<T>(string key, T value);

        void Add<T>(string key, T value, int duration);

        void Clear(string key);
    }
}