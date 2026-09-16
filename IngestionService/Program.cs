using IngestionService;
using IngestionService.Models.Configurations;
using IngestionService.Models.DTOs;
using IngestionService.Orchestrators;
using IngestionService.Services;

var builder = Host.CreateApplicationBuilder(args);

// configuration:

// api
StationsAPIs stationsAPIs = new StationsAPIs
{
};
builder.Configuration.GetSection("StationsAPIs").Bind(stationsAPIs);
builder.Services.AddSingleton(stationsAPIs);

// kafka connection
KafkaConnectionConfiguration kafkaConnectionConfiguration = new KafkaConnectionConfiguration
{
};
builder.Configuration.GetSection("KafkaConnectionConfiguration").Bind(kafkaConnectionConfiguration);
builder.Services.AddSingleton(kafkaConnectionConfiguration);

// kafka topics 

KafkaTopicsConfiguration kafkaTopicsConfiguration = new KafkaTopicsConfiguration
{
};
builder.Configuration.GetSection("KafkaTopicsConfiguration").Bind(kafkaTopicsConfiguration);
builder.Services.AddSingleton(kafkaTopicsConfiguration);

// services configuration

ServicesConfiguration servicesConfiguration = new ServicesConfiguration
{
};
builder.Configuration.GetSection("ServicesConfiguration").Bind(servicesConfiguration);
builder.Services.AddSingleton(servicesConfiguration);


// services 
builder.Services.AddHttpClient();
builder.Services.AddSingleton<StationApiService>();
builder.Services.AddSingleton<KafkaService>();
builder.Services.AddSingleton<IGBFSValidator , GBFSValidator>();

// workers

builder.Services.AddHostedService<StationInformationService>();
builder.Services.AddHostedService<StationStatusService>();
builder.Services.AddHostedService<VehicleTypesService>();

// run

var host = builder.Build();
host.Run();
