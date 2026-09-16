using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver.Core.Configuration;

namespace ProcessingService.DAL
{
    public class MysqlConfigs
    {
        public string ConnectionString {get; set;} = string.Empty;
    }
}