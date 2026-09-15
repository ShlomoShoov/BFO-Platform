using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IngestionService.Models.Configurations
{
    public class KafkaConnectionConfiguration
    {
        public string KafkaBootStrapServes { get; set; } = string.Empty;
    }
}