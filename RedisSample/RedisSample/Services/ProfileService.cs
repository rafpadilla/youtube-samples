using StackExchange.Redis;

namespace RedisSample.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public ProfileService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }
        public async Task Add(string key, string value)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await db.StringSetAsync(key, value);
        }

        public async Task<string> Get(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            return await db.StringGetAsync(key);
        }
    }
}
