using ProcessingService.DAL;

namespace ProcessingService;

public class Worker : BackgroundService
{
    private KafkaContext _kafkaContext;
    private MongoDbContext _mongoDbContext;
    private RedisContext _redisContext;
    public Worker(KafkaContext kafkaContext, MongoDbContext mongoDbContext, RedisContext redisContext)
    {
        _kafkaContext = kafkaContext;
        _mongoDbContext =  mongoDbContext;
        _redisContext = redisContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
        }
    }
}
