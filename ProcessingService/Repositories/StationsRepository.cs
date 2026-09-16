using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProcessingService.DAL;

namespace ProcessingService.Repositories
{
    public class StationsRepository
    {
        private KafkaContext _kafkaContext;
        private MongoDbContext _mongoDbContext;
        private MySqlDbContext _mySqlDbContext;
        private RedisContext _redisContext;
        public StationsRepository(KafkaContext kafkaContext, MongoDbContext mongoDbContext, MySqlDbContext mySqlDbContext, RedisContext redisContext)
        {
            _kafkaContext = kafkaContext;
            _mongoDbContext = mongoDbContext;
            _mySqlDbContext = mySqlDbContext;
            _redisContext = redisContext;
        }
    }
}