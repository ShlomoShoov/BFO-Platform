using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IngestionService.Models.Configurations;
using IngestionService.Models.DTOs;
using IngestionService.Services;

namespace IngestionService.Orchestrators
{
    public class VehicleTypesService :BaseOrchestratorService
    {
        public VehicleTypesService(ILogger<BaseOrchestratorService> logger, KafkaTopicsConfiguration kafkaTopics, ServicesConfiguration servicesConfiguration, IGBFSValidator GBFSValidator, KafkaService kafkaService, StationApiService apiService) : base(logger, kafkaTopics, servicesConfiguration, GBFSValidator, kafkaService, apiService)
        {
        }

        protected override int MinutesTrigger { get => _servicesConfiguration.VehicleTypesServiceMinuetsTrigger; }

        protected override string ServiceName => "VehicleTypesService";

        protected override string KafkaTopic => _kafkaTopics.VehicleTypesTopicName;

        protected override async Task<IEnumerable<IDTO>> GetDTOs()
        {
            return await _apiService.GetVehicleTypesAsync();
        }
    }
}
