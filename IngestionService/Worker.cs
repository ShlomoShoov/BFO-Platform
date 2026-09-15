using IngestionService.Services;

namespace IngestionService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private StationApiService _stationApiService;

    public Worker(ILogger<Worker> logger, StationApiService stationApiService)
    {
        _logger = logger;
        _stationApiService = stationApiService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        System.Console.WriteLine($"trying station information");
        System.Console.WriteLine( string.Join(",",(await _stationApiService.GetStationsInformationAsync()).Select(s=> s.StationId).ToList()));
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
