using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ProcessingService.Models.Configurations;
using StackExchange.Redis;

namespace ProcessingService.DAL
{
    public class RedisContext
    {
        private RedisConfigs _redisConfigs;
        private ConnectionMultiplexer? _connectionMultiplexer;
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
            if (_connectionMultiplexer == null)
            {
                _connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(_redisConfigs.ConnectionString);
            }
            return _connectionMultiplexer.GetDatabase();
        }
    }
}