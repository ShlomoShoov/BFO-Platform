using IngestionService;
using IngestionService.Models.Configurations;
using IngestionService.Services;

var builder = Host.CreateApplicationBuilder(args);

// configuration:
StationsAPIs stationsAPIs = new StationsAPIs
{
    StationInformationAPI = "https://gbfs.lyft.com/gbfs/2.3/bkn/en/station_information.json",
    StationInformationKeyPath = ["data", "stations"],
    StationStatusAPI = "https://gbfs.lyft.com/gbfs/2.3/bkn/en/station_status.json",
    StationStatusKeyPath = ["data", "stations"],
    VehicleTypesAPI = "https://gbfs.lyft.com/gbfs/2.3/bkn/en/vehicle_types.json",
    VehicleTypesKeyPath = ["data", "vehicle_types"]

};
System.Console.WriteLine(stationsAPIs.StationInformationAPI);

builder.Configuration.GetSection("StationsAPIs").Bind(stationsAPIs);
builder.Services.AddSingleton(stationsAPIs);


// services

builder.Services.AddHostedService<Worker>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<StationApiService>();
builder.Services.AddSingleton<KafkaService>();


var host = builder.Build();
host.Run();
