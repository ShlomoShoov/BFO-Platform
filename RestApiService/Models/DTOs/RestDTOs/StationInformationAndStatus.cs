using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiService.Models.DTOs.RestDTOs
{
    public class StationInformationAndStatus
    {
        public string StationId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double Lon { get; set; }
        public double Lat { get; set; }
        public int Capacity { get; set; }
        public int? NumBikesAvailable { get; set; }
        public int? NumDocksAvailable { get; set; }
        public bool? IsRenting { get; set; }
        public bool? IsReturning { get; set; }
        public long? LastReported { get; set; }
    }
}