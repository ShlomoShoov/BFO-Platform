using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiService.Models.DTOs.RestDTOs
{
    public class DashboardDTO
    {
        public int TotalStations { get; set; }
        public int EmptyStations { get; set; }
        public int FullStations { get; set; }
        public int LowAvailabilityStations { get; set; }
        public int OutOfServiceStations { get; set; }
        public long? LastReported { get; set; }


    }
}