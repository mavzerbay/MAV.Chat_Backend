using MAV.Chat.Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace MAV.Chat.Infrastructure.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;
        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timetoLive)
        {
            if (response == null) return;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var serialisedResponse = JsonSerializer.Serialize(response, options);

            await _database.StringSetAsync(cacheKey, serialisedResponse, timetoLive);
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            var cachedResponse = await _database.StringGetAsync(cacheKey);

            if (cachedResponse.IsNullOrEmpty)
            {
                return null;
            }

            return cachedResponse;
        }
    }
}
