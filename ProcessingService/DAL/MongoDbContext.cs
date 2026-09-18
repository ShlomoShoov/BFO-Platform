using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using ProcessingService.Models.Configurations;
using ProcessingService.Models.DTOs;

namespace ProcessingService.DAL
{
    public class MongoDbContext
    {
        public IMongoCollection<StationStatusDTO> StationsStatuses;
        
        public MongoDbContext(MongoConfiguration mongoConfiguration)
        {
            StationsStatuses = new MongoClient(mongoConfiguration.ConnectionString).
                GetDatabase(mongoConfiguration.DatabaseName).
                GetCollection<StationStatusDTO>(mongoConfiguration.StationStatusCollectionName);
        }
    }
}