using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IngestionService.Models.Configurations
{
    public class KafkaTopicsConfiguration
    {
        public string StationInformationTopicName { get; set; } = string.Empty;
        public string StationStatusTopicName { get; set; } = string.Empty;
        public string VehicleTypesTopicName { get; set; } = string.Empty;

    }
}