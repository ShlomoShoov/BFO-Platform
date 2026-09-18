using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using ProcessingService.Exceptions;
using ProcessingService.Models.Configurations;
using ProcessingService.Models.DTOs;

namespace ProcessingService.Services
{
    public class EventsConvertor : IEventsConvertor
    {
        private KafkaTopicsConfiguration _kafkaTopics;
        public EventsConvertor(KafkaTopicsConfiguration kafkaTopics)
        {
            _kafkaTopics = kafkaTopics;
        }
        public IDTO GetDtoFromConsumeResult(ConsumeResult<Null, string> consumeResult)
        {
            if (consumeResult.Topic == _kafkaTopics.StationInformationTopicName)
            {
                return _Convert<StationInformationDTO>(consumeResult.Message.Value);
            }
            if (consumeResult.Topic == _kafkaTopics.StationStatusTopicName)
            {
                return _Convert<StationStatusDTO>(consumeResult.Message.Value);
            }
            if (consumeResult.Topic == _kafkaTopics.VehicleTypesTopicName)
            {
                return _Convert<VehicleTypesDTO>(consumeResult.Message.Value);
            }
            throw new NotImplementedException();

        }

        private T _Convert<T>(string rawJson)
        {
            try
            {
                T? result =  JsonSerializer.Deserialize<T>(rawJson);
                if (result == null)
                {
                    throw new ConvertorException($"trying to convert to {typeof(T).Name}: {rawJson}, but got Null.");
                }
                return result;
            }
            catch (JsonException ex)
            {
                throw new ConvertorException(ex.Message);
            }
        }
    }
}