using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver.Core.Configuration;

namespace RestApiService.Models.Configurations
{
    public class MysqlConfigs
    {
        public string ConnectionString {get; set;} = string.Empty;
    }
}