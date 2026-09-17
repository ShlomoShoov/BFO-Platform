using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MongoDB.Driver;
using RestApiService.DAL;
using RestApiService.Models.DTOs;
using SharpCompress.Compressors.ZStandard.Unsafe;

namespace RestApiService.Repositories
{
    public class StationsRepository : IStationsRepository
    {
        private MongoDbContext _mongoDbContext;
        private MySqlDbContext _mySqlDbContext;
        private RedisContext _redisContext;
        private ILogger<StationsRepository> _logger;
        public StationsRepository(MongoDbContext mongoDbContext, MySqlDbContext mySqlDbContext, RedisContext redisContext, ILogger<StationsRepository> logger)
        {
            _mongoDbContext = mongoDbContext;
            _mySqlDbContext = mySqlDbContext;
            _redisContext = redisContext;
            _logger = logger;
        }
        public async Task Init()
        {
            await _mySqlDbContext.Database.MigrateAsync();
        }

        
       
    }
}