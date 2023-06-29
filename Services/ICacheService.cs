namespace NhonOJT_redis.Services
{
    public interface ICacheService
    {
        Task<T> Get<T>(string key);
        Task<bool> Remove(string key);
        Task<bool> Set<T>(string key, T value, DateTimeOffset expirationTime);
    }

}