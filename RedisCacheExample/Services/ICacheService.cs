namespace RedisCacheExample.Services
{
    public interface ICacheService
    {
        T GetData<T>(string key);
        object RemoveData(string key);
        bool SetData<T>(string key, T value, DateTimeOffset expirationTime);
    }
}
