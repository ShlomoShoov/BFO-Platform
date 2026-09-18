using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MongoDB.Driver;
using ProcessingService.DAL;
using ProcessingService.Models.DTOs;
using SharpCompress.Compressors.ZStandard.Unsafe;

namespace ProcessingService.Repositories
{
    public class StationsRepository : IStationsRepository
    {
        private MongoDbContext _mongoDbContext;
        private MySqlDbContext _mySqlDbContext;
        private RedisContext _redisContext;
        private ILogger<StationsRepository> _logger;
        public StationsRepository(MongoDbContext mongoDbContext, MySqlDbContext mySqlDbContext, RedisContext redisContext, ILogger<StationsRepository> logger)
        {
            _mongoDbContext = mongoDbContext;
            _mySqlDbContext = mySqlDbContext;
            _redisContext = redisContext;
            _logger = logger;
        }
        public async Task Init()
        {
            await _mySqlDbContext.Database.MigrateAsync();
        }
        public async Task AddAsync(IDTO DTO)
        {
            switch(DTO)
            {
                case StationInformationDTO dto:
                    await _AddInformation(dto);
                    break;
                case VehicleTypesDTO dto:
                    await _AddVehicleType(dto);
                    break;
                case StationStatusDTO dto:
                    await _AddStatus(dto);
                    break;
                default:
                    throw new NotImplementedException();
            }
            ;
        }
        private async Task _AddInformation(StationInformationDTO stationInformation)
        {
            _logger.LogInformation($"requests to add station info for station id: {stationInformation.StationId}");
            StationInformationDTO? existsStationInfo = await _mySqlDbContext.StationsInformation.FirstOrDefaultAsync(s=> s.StationId == stationInformation.StationId);
            if (existsStationInfo!=null)
            {
                _logger.LogInformation($"{stationInformation.StationId} exists. updating...");

                // updating
                existsStationInfo.Name = stationInformation.Name;
                existsStationInfo.Lat = stationInformation.Lat;
                existsStationInfo.Lon = stationInformation.Lon;
                existsStationInfo.Capacity = stationInformation.Capacity;
            }
            else
            {
                //adding
                _logger.LogInformation($"{stationInformation.StationId} - not exists - adding");
                _mySqlDbContext.StationsInformation.Add(stationInformation);
            }
            // saving
            await _mySqlDbContext.SaveChangesAsync();
        }

        private async Task _AddVehicleType(VehicleTypesDTO vehicleType)
        {
            _logger.LogInformation($"Request to add vehicle type: {vehicleType.VehicleTypeId}");
            VehicleTypesDTO? existsVehicleType = await _mySqlDbContext.VehicleTypes.FirstOrDefaultAsync(v=> v.VehicleTypeId == vehicleType.VehicleTypeId);
            if (existsVehicleType != null)
            {
                _logger.LogInformation($"{vehicleType.VehicleTypeId} exists. updating...");
                 
                // updating 

                existsVehicleType.FormFactor = vehicleType.FormFactor;
                existsVehicleType.PropulsionType = vehicleType.PropulsionType;
            }
            else
            {
                _logger.LogInformation($"{vehicleType.VehicleTypeId} - not exists - adding");
                _mySqlDbContext.VehicleTypes.Add(vehicleType);

            }
            await _mySqlDbContext.SaveChangesAsync();
        }

        private async Task _AddStatus(StationStatusDTO stationStatus)
        {
            _logger.LogInformation($"Request to add station status, Id:{stationStatus.StationId}");
            bool stationExists = await  _mySqlDbContext.StationsInformation.AnyAsync(s=> s.StationId == stationStatus.StationId);
             
            if (!stationExists)
            {
                _logger.LogWarning($"Station Information for station {stationStatus.StationId} Is NOT exists. Not added");
                return;
            }
            string redisKey = "station_status:"+stationStatus.StationId;
            var redisDataBase = await _redisContext.GetDataBaseConnectionAsync();
            string? value = await redisDataBase.StringGetAsync(redisKey);
            if (!string.IsNullOrWhiteSpace(value))
            {
                _logger.LogInformation("Found exists Id on Redis");
                StationStatusDTO statusOnRedis = JsonSerializer.Deserialize<StationStatusDTO>(value)!;
                if (_IsTowStatusesEqual(stationStatus, statusOnRedis))
                {
                    _logger.LogInformation("Both status are the same, not updating database");
                    return;
                }
            }
            _logger.LogInformation($"Saving new data to redis, mongo and database");
            string statusAsJson = JsonSerializer.Serialize(stationStatus);
            await redisDataBase.StringSetAsync(redisKey, statusAsJson);
            await _mongoDbContext.StationsStatuses.InsertOneAsync(stationStatus);

            // updating in sql last update
            StationStatusDTO? existsStationStatus = await _mySqlDbContext.StationStatuses.FirstOrDefaultAsync(s=> s.StationId == stationStatus.StationId);
            if (existsStationStatus == null)
            {
                _mySqlDbContext.StationStatuses.Add(stationStatus);
            }
            else
            {
                existsStationStatus.IsRenting =  stationStatus.IsRenting;
                existsStationStatus.IsReturning = stationStatus.IsReturning;
                existsStationStatus.LastReported = stationStatus.LastReported;
                existsStationStatus.NumBikesAvailable = stationStatus.NumBikesAvailable;
                existsStationStatus.NumDocksAvailable = stationStatus.NumDocksAvailable;
            }
            // save changes
            await _mySqlDbContext.SaveChangesAsync();
            
            

        }

        private bool _IsTowStatusesEqual(StationStatusDTO status1, StationStatusDTO status2)
        {
            return status1.IsRenting == status2.IsRenting &&
                    status1.IsReturning == status2.IsReturning &&
                    status1.NumBikesAvailable == status2.NumBikesAvailable &&
                    status1.NumDocksAvailable == status2.NumDocksAvailable;
        }

        


    }
}