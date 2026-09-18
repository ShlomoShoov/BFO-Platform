using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessingService.Models.Configurations
{
    public class MongoConfiguration
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string StationStatusCollectionName { get; set; } = null!;
    }
}