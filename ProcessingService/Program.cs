using Microsoft.EntityFrameworkCore;
using ProcessingService;
using ProcessingService.DAL;
using ProcessingService.Models.Configurations;
using ProcessingService.Orchestrators;
using ProcessingService.Repositories;
using ProcessingService.Services;

var builder = Host.CreateApplicationBuilder(args);

// configuration:


// kafka connection
KafkaConnectionConfiguration kafkaConnectionConfiguration = new KafkaConnectionConfiguration
{
    // KafkaBootStrapServes = "localhost:9095",
    // ConsumerGroupId = "test-group-id"
};
builder.Configuration.GetSection("KafkaConnectionConfiguration").Bind(kafkaConnectionConfiguration);
builder.Services.AddSingleton(kafkaConnectionConfiguration);
builder.Services.AddSingleton<KafkaContext>();

// kafka topics 

KafkaTopicsConfiguration kafkaTopicsConfiguration = new KafkaTopicsConfiguration
{
    // StationInformationTopicName = "bike.station-information",
    // StationStatusTopicName = "bike.station-status",
    // VehicleTypesTopicName = "bike.vehicle-types"
};
builder.Configuration.GetSection("KafkaTopicsConfiguration").Bind(kafkaTopicsConfiguration);
builder.Services.AddSingleton(kafkaTopicsConfiguration);

// mysql 

MysqlConfigs mysqlConfigs = new MysqlConfigs()
{
    // ConnectionString = "Server=localhost;Port=3306;Password=1234;User=root;Database=gbfs-db"
};
builder.Configuration.GetSection("MysqlConfigs").Bind(mysqlConfigs);
builder.Services.AddDbContext<MySqlDbContext>(options=> options.UseMySql(mysqlConfigs.ConnectionString, ServerVersion.AutoDetect(mysqlConfigs.ConnectionString)));


// mongo
MongoConfiguration mongoConfiguration = new MongoConfiguration()
{
    // ConnectionString = "mongodb://root:1234@localhost:27017",
    // DatabaseName = "gbfs-db",
    // StationStatusCollectionName = "reports-status"
};
builder.Configuration.GetSection("MongoConfiguration").Bind(mongoConfiguration);
builder.Services.AddSingleton(mongoConfiguration);
builder.Services.AddSingleton<MongoDbContext>();

//redis
RedisConfigs redisConfigs = new RedisConfigs
{
    // ConnectionString = "localhost:6379"
};
builder.Configuration.GetSection("RedisConfigs").Bind(redisConfigs);
builder.Services.AddSingleton(redisConfigs);
builder.Services.AddSingleton<RedisContext>();


// services
builder.Services.AddScoped<IStationsRepository, StationsRepository>();
builder.Services.AddSingleton<IEventsConvertor, EventsConvertor>();

// worker
builder.Services.AddHostedService<ProcessingOrchestrator>();


var host = builder.Build();
host.Run();
