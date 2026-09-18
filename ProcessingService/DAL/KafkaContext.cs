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
        private KafkaTopicsConfiguration _kafkaTopics;

        public KafkaContext(KafkaConnectionConfiguration kafkaConfigs, KafkaTopicsConfiguration kafkaTopics)
        {
            _kafkaTopics = kafkaTopics;
            ConsumerConfig consumerConfigs = new ConsumerConfig
            {
                BootstrapServers = kafkaConfigs.KafkaBootStrapServes,
                GroupId = kafkaConfigs.ConsumerGroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };
            Consumer = new ConsumerBuilder<Null, string>(consumerConfigs).Build();
        }

        public void Init()
        {
            Consumer.Subscribe([_kafkaTopics.StationInformationTopicName, _kafkaTopics.StationStatusTopicName, _kafkaTopics.VehicleTypesTopicName]);
        }

        public void Dispose()
        {
            Consumer.Unsubscribe();
            Consumer.Dispose();
        }
    }
}