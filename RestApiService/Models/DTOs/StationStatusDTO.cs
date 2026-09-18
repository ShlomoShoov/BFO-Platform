using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RestApiService.Models.DTOs
{
    public class StationStatusDTO:IDTO
    {
        public string StationId { get; set; } = string.Empty;
        public int NumBikesAvailable { get; set; }
        public int NumDocksAvailable { get; set; }
        public bool IsRenting {  get; set; }
        public bool IsReturning {  get; set; }
        public long LastReported { get; set; }
        public StationInformationDTO StationInformation = null!;

    }
}