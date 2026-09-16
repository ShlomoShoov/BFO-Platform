using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProcessingService.Models.Configurations;
using StackExchange.Redis;

namespace ProcessingService.DAL
{
    public class RedisContext
    {
        private RedisConfigs _redisConfigs;
        public RedisContext(RedisConfigs redisConfigs)
        {
            _redisConfigs = redisConfigs;
        }

        public async Task<IDatabase> GetDataBaseConnectionAsync()
        {
            ConnectionMultiplexer connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(_redisConfigs.ConnectionString);
            return connectionMultiplexer.GetDatabase();
        }
    }
}