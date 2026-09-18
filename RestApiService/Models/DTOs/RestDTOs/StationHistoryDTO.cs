using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiService.Models.DTOs.RestDTOs
{
    public class StationHistoryDTO
    {
        public string StationId { get; set; } = string.Empty;
        public int NumBikesAvailable { get; set; }
        public int NumDocksAvailable { get; set; }
        public long Timestamp { get; set; }
    }
}