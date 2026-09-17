using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using ProcessingService.DAL;
using ProcessingService.Models.DTOs;
using ProcessingService.Repositories;
using ProcessingService.Services;

namespace ProcessingService.Orchestrators
{
    public class ProcessingOrchestrator : BackgroundService
    {

        private KafkaContext _kafkaContext;
        private IEventsConvertor _eventsConvertor;
        private IServiceProvider _serviceProvider;
        private ILogger<ProcessingOrchestrator> _logger;
        
        public ProcessingOrchestrator(KafkaContext kafkaContext, IEventsConvertor eventsConvertor, ILogger<ProcessingOrchestrator> logger, IServiceProvider serviceProvider)
        {
            _kafkaContext = kafkaContext;
            _eventsConvertor = eventsConvertor;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        private async Task _Init()
        {
            _kafkaContext.Init();
            using var scope = _serviceProvider.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IStationsRepository>();
            await repository.Init();
        } 
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // init
            _logger.LogInformation("Orchestrator Triggered, Init kafka and DBs");
            await _Init();
            _logger.LogInformation("Init Complete");
            
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // consume and convert
                    ConsumeResult<Null, string> consumeResult = _kafkaContext.Consumer.Consume(stoppingToken);
                    IDTO DTO = _eventsConvertor.GetDtoFromConsumeResult(consumeResult);

                    // create repos scope 
                    using var scope = _serviceProvider.CreateScope();
                    var repository = scope.ServiceProvider.GetRequiredService<IStationsRepository>();

                    // save and commit
                    await repository.AddAsync(DTO);
                    _kafkaContext.Consumer.Commit(consumeResult);
                }
                catch(Exception ex)
                {
                    _GlobalErrorHandler(ex);
                }

            }
        }

        private void  _GlobalErrorHandler(Exception exception)
        {
            _logger.LogError(exception, "Error in the main consuming loop");
        }
    }
}