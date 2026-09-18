using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using IngestionService.Exceptions;
using IngestionService.Models.Configurations;
using IngestionService.Models.DTOs;
using IngestionService.Models.Results;
using IngestionService.Services;

namespace IngestionService.Orchestrators
{
    public abstract class BaseOrchestratorService : BackgroundService
    {
        protected readonly ILogger<BaseOrchestratorService> _logger;
        // configs
        protected KafkaTopicsConfiguration _kafkaTopics;
        protected ServicesConfiguration _servicesConfiguration;
        // services
        protected IGBFSValidator _GBFSValidator;
        protected KafkaService _kafkaService;
        protected StationApiService _apiService;


        // Force Children Classes to implement so the loop will works

        protected abstract int MinutesTrigger {get;}
        protected abstract string ServiceName {get;}
        protected abstract string KafkaTopic { get; }

        protected abstract Task<IEnumerable<IDTO>> GetDTOs(); 
        // constructor 
        public BaseOrchestratorService(ILogger<BaseOrchestratorService> logger, KafkaTopicsConfiguration kafkaTopics
                                        , ServicesConfiguration servicesConfiguration, IGBFSValidator GBFSValidator
                                        , KafkaService kafkaService, StationApiService apiService)
        {
            _logger = logger;
            _kafkaTopics = kafkaTopics;
            _servicesConfiguration = servicesConfiguration;
            _GBFSValidator = GBFSValidator;
            _kafkaService = kafkaService;
            _apiService = apiService;

        }

        // loop and error handle
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer periodicTimer = new PeriodicTimer(TimeSpan.FromMinutes(MinutesTrigger));
            bool isFirstTime = true;
            while (isFirstTime || await periodicTimer.WaitForNextTickAsync(stoppingToken))
            {
                if (isFirstTime)
                {
                    _logger.LogInformation($"Starting The loop for service {ServiceName}, will trigger again every {MinutesTrigger} minutes");
                    isFirstTime = false;
                }
                
                DateTime startTime = DateTime.UtcNow;
                _logger.LogInformation($"Trigger turned for service : {ServiceName} (every {MinutesTrigger} minutes)");
                try
                {
                    IEnumerable<IDTO> DTOs = await GetDTOs();
                    double totalTimeForBringDTO = ( DateTime.UtcNow- startTime).TotalSeconds;
                    _logger.LogInformation($"{ServiceName} - Get from api successfully {DTOs.Count()} Items. Took {totalTimeForBringDTO} seconds");
                    
                    List<string> offsets = [];
                    foreach(IDTO dto in DTOs)
                    {
                        GBFSValidationResult validationResult = _GBFSValidator.Validate(dto);
                        if (!validationResult.IsSuccess)
                        {
                            _logger.LogWarning($"Validation Failed: {validationResult.Reason}");
                        }
                        string jsonDTO = JsonSerializer.Serialize(dto, dto.GetType());
                        Message<Null, string> message  = new Message<Null, string>
                        {
                            Value = jsonDTO
                        };

                        DeliveryResult<Null, string> result = await _kafkaService.Producer.ProduceAsync(KafkaTopic, message);
                        offsets.Add(result.Offset.ToString());
                    }
                    double totalTimeForKafkaProducing = (DateTime.UtcNow - startTime).TotalSeconds - totalTimeForBringDTO;
                    _logger.LogInformation($"Produce To kafka {offsets.Count} Items, Took {totalTimeForKafkaProducing} seconds\n"+
                                            $"offsets produce: [{string.Join(",", offsets)}]");

                }
                catch (Exception ex)
                {
                    _GlobalErrorHandler(ex);
                } 
            }
        }

        private void _GlobalErrorHandler(Exception ex)
        {
            string logMessage = ex switch
            {
                ApiConnectionException apiEx => $"Error while Connection to api or url {apiEx.Url}",
                ApiResponseException apiReEX => $"Request Not Success. status code: {apiReEX.StatusCode} | Content: {apiReEX.Content}", 
                DeserializeException desEx => $"Error While Trying Convert Json. Try To convert To {desEx.ObjectName} | json: {desEx.RawText}",
                _=> "Unknown Error!"
            };
            logMessage += $"| Message: {ex.Message}";
            _logger.LogError(ex, logMessage);
        }
    }
}