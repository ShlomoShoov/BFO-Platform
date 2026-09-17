using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiService.Models.DTOs
{
    public class StationInformationDTO : IDTO
    {
        public string StationId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double Lon { get; set; }
        public double Lat { get; set; }
        public int Capacity { get; set; }
        public StationStatusDTO? StationStatus = null;

    }
}