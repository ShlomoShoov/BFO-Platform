using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestApiService.Models.DTOs;
using RestApiService.Models.DTOs.RestDTOs;

namespace RestApiService.Repositories
{
    public interface IStationsRepository
    {
        public  Task Init();
        public  Task<IEnumerable<StationInformationAndStatus>> GetStationInformationAndStatusesAsync(int? minAvailableBikes, bool? isRenting, bool? isReturning);
        public Task<StationInformationAndStatus?> GetStationInformationAndStatusesByIdAsync(string id);

        public  Task<StationStatusOnlyDTO?> GetStationStatusAsync(string id);
        public  Task<IEnumerable<StationHistoryDTO>> GetHistoryByIdAsync(string id, DateTime? from, DateTime? to, int? limit);
        public  Task<DashboardDTO> GetDashboardAsync();




    }
}