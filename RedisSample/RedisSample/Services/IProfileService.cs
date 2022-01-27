namespace RedisSample.Services
{
    public interface IProfileService
    {
        Task Add(string key, string value);
        Task<string> Get(string key);
    }
}
