using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MongoDB.Driver;
using RestApiService.DAL;
using RestApiService.Models.DTOs;
using RestApiService.Models.DTOs.RestDTOs;
using SharpCompress.Compressors.ZStandard.Unsafe;

namespace RestApiService.Repositories
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

        public async Task<IEnumerable<StationInformationAndStatus>> GetStationInformationAndStatusesAsync(int? minAvailableBikes, bool? isRenting, bool? isReturning)
        {
            IQueryable<StationInformationDTO> query = _mySqlDbContext.StationsInformation;
            if (minAvailableBikes.HasValue)
            {
                query = query.Where(s=> s.StationStatus!= null && s.StationStatus.NumBikesAvailable>=minAvailableBikes);
            }
            if (isRenting.HasValue)
            {
                query = query.Where(s=> s.StationStatus != null && s.StationStatus.IsRenting == isRenting);
            }
            if (isReturning.HasValue)
            {
                query = query.Where(s => s.StationStatus != null && s.StationStatus.IsReturning == isReturning);
            }
            return await DtoiseStation(query).ToListAsync();
        }
        public  async Task<StationInformationAndStatus?> GetStationInformationAndStatusesByIdAsync(string id)
        {
            return await DtoiseStation(_mySqlDbContext.StationsInformation).FirstOrDefaultAsync(s=> s.StationId == id);

        }

        public async Task<StationStatusOnlyDTO?> GetStationStatusAsync(string id)
        {
            return await _mySqlDbContext.StationStatuses.Select(s=> 
                                                    new StationStatusOnlyDTO
                                                    {
                                                        StationId = s.StationId,
                                                        IsRenting = s.IsRenting,
                                                        IsReturning = s.IsReturning,
                                                        LastReported = s.LastReported,
                                                        NumBikesAvailable = s.NumBikesAvailable,
                                                        NumDocksAvailable = s.NumDocksAvailable
                                                    }).FirstOrDefaultAsync(s=> s.StationId == id);
        }

        

        private IQueryable<StationInformationAndStatus> DtoiseStation(IQueryable<StationInformationDTO> stationsInformation)
        {
            return stationsInformation.Select(s => new StationInformationAndStatus
            {
                StationId = s.StationId,
                Capacity = s.Capacity,
                Name = s.Name,
                Lat = s.Lat,
                Lon = s.Lon,
                LastReported = s.StationStatus != null ? s.StationStatus.LastReported : null,
                IsRenting = s.StationStatus != null ? s.StationStatus.IsRenting : null,
                IsReturning = s.StationStatus != null ? s.StationStatus.IsReturning : null,
                NumBikesAvailable = s.StationStatus != null ? s.StationStatus.NumBikesAvailable : null,
                NumDocksAvailable = s.StationStatus != null ? s.StationStatus.NumDocksAvailable : null,
            });
        }

        public async Task<IEnumerable<StationHistoryDTO>> GetHistoryByIdAsync(string id, DateTime? from, DateTime? to, int? limit)
        {
            FilterDefinitionBuilder<StationStatusDTO> builder = Builders<StationStatusDTO>.Filter;
            List<FilterDefinition<StationStatusDTO>> filterList = new List<FilterDefinition<StationStatusDTO>>();

            if (from.HasValue)
            {
                filterList.Add(builder.Gte(s=> s.LastReported, _ConvertDateTimeToTimeStamp(from.Value)));
            }
            if (to.HasValue)
            {
                filterList.Add(builder.Lte(s => s.LastReported, _ConvertDateTimeToTimeStamp( to.Value)));
            }
            filterList.Add(builder.Eq(s=> s.StationId ,id));

            
            var finalBuilder = builder.And(filterList);
            var query =  _mongoDbContext.StationsStatuses.Find(finalBuilder);
            
            if (limit.HasValue)
            {
                query = query.Limit(limit);
            }
            
            var finalQuery = query.Project(s=> new StationHistoryDTO
            {
                StationId = s.StationId,
                NumBikesAvailable = s.NumBikesAvailable,
                NumDocksAvailable = s.NumDocksAvailable,
                Timestamp = s.LastReported
            });

            return await finalQuery.ToListAsync();
        }

        public async Task<DashboardDTO> GetDashboardAsync()
        {
            DashboardDTO result =  await _mySqlDbContext.StationsInformation
                        .GroupBy(s=> 1)
                        .Select(g=> new DashboardDTO
                        {
                            TotalStations = g.Count(),
                            EmptyStations = g.Count(s=> s.StationStatus!= null && s.StationStatus.NumBikesAvailable==0),
                            FullStations = g.Count(s => s.StationStatus != null && s.StationStatus.NumBikesAvailable == s.Capacity),
                            LowAvailabilityStations = g.Count(s => s.StationStatus != null && s.StationStatus.NumBikesAvailable < 5),
                            OutOfServiceStations = g.Count(s => s.StationStatus != null && !s.StationStatus.IsReturning),
                            LastReported = g.Where(s=> s.StationStatus!= null).Select(s=> s.StationStatus!.LastReported).FirstOrDefault()
                        }).
                        FirstOrDefaultAsync() ?? new DashboardDTO();
            return result;

        }
        private long _ConvertDateTimeToTimeStamp(DateTime dateTime)
        {
            long result = new DateTimeOffset(dateTime).ToUnixTimeSeconds();
            return result;
        }
        private DateTime _ConvertTimeStampToDateTime(long timeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timeStamp).UtcDateTime;
        }
        public async Task Init()
        {
            await _mySqlDbContext.Database.MigrateAsync();
        }

        
       
    }
}