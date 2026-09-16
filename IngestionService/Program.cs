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
    StationInformationAPI = "https://gbfs.lyft.com/gbfs/2.3/bkn/en/station_information.json",
    StationInformationKeyPath = ["data", "stations"],
    StationStatusAPI = "https://gbfs.lyft.com/gbfs/2.3/bkn/en/station_status.json",
    StationStatusKeyPath = ["data", "stations"],
    VehicleTypesAPI = "https://gbfs.lyft.com/gbfs/2.3/bkn/en/vehicle_types.json",
    VehicleTypesKeyPath = ["data", "vehicle_types"]

};
builder.Configuration.GetSection("StationsAPIs").Bind(stationsAPIs);
builder.Services.AddSingleton(stationsAPIs);

// kafka connection
KafkaConnectionConfiguration kafkaConnectionConfiguration = new KafkaConnectionConfiguration
{
    KafkaBootStrapServes = "localhost:9095"
};
builder.Configuration.GetSection("KafkaConnectionConfiguration").Bind(kafkaConnectionConfiguration);
builder.Services.AddSingleton(kafkaConnectionConfiguration);

// kafka topics 

KafkaTopicsConfiguration kafkaTopicsConfiguration = new KafkaTopicsConfiguration
{
    StationInformationTopicName = "bike.station-information",
    StationStatusTopicName = "bike.station-status",
    VehicleTypesTopicName = "bike.vehicle-types"
};
builder.Configuration.GetSection("KafkaTopicsConfiguration").Bind(kafkaTopicsConfiguration);
builder.Services.AddSingleton(kafkaTopicsConfiguration);

// services configuration

ServicesConfiguration servicesConfiguration = new ServicesConfiguration
{
     StationInformationServiceMinuetsTrigger = 60,
     StationStatusServiceMinuetsTrigger = 1,
     VehicleTypesServiceMinuetsTrigger = 60  
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
