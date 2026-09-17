using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using RestApiService.Models.Configurations;
using StackExchange.Redis;

namespace RestApiService.DAL
{
    public class RedisContext
    {
        private RedisConfigs _redisConfigs;
        public RedisContext(RedisConfigs redisConfigs)
        {
            _redisConfigs = redisConfigs;
        }

        public async Task<T?> GetByKeyAsync<T>(string key)
        {
            string? value = await (await GetDataBaseConnectionAsync()).StringGetAsync(key);
            if (string.IsNullOrWhiteSpace(value))
            {
                return default;
            }
            T serializedValue = JsonSerializer.Deserialize<T>(value)!;
            return serializedValue;
        }

        public async Task<IDatabase> GetDataBaseConnectionAsync()
        {
            ConnectionMultiplexer connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(_redisConfigs.ConnectionString);
            return connectionMultiplexer.GetDatabase();
        }
    }
}