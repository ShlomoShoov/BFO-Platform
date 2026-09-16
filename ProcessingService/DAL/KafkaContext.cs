using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using ProcessingService.Models.Configurations;

namespace ProcessingService.DAL
{
    public class KafkaContext  : IDisposable
    {
        public IConsumer<Null, string> Consumer;
        public KafkaContext(KafkaConnectionConfiguration kafkaConfigs)
        {
            ConsumerConfig consumerConfigs = new ConsumerConfig
            {
                BootstrapServers = kafkaConfigs.KafkaBootStrapServes,
                GroupId = kafkaConfigs.ConsumerGroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };
            Consumer = new ConsumerBuilder<Null, string>(consumerConfigs).Build();
        }

        public void Dispose()
        {
            Consumer.Dispose();
        }
    }
}