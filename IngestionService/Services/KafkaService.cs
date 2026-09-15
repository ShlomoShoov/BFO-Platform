using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using IngestionService.Models.Configurations;

namespace IngestionService.Services
{
    public class KafkaService
    {
        private ILogger<KafkaService> _logger;
        public IProducer<Null, string> Producer {get;}

        public KafkaService(ILogger<KafkaService> logger, KafkaConnectionConfiguration kafkaConnectionConfiguration)
        {
            _logger = logger;
            ProducerConfig configs = new ProducerConfig()
            {
                BootstrapServers = kafkaConnectionConfiguration.KafkaBootStrapServes
            };
            Producer = new ProducerBuilder<Null, string>(configs).Build();
        }

        
    }
}