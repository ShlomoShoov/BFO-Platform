using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IngestionService.Models.DTOs
{
    public class StationInformationDTO
    {
        public string StationId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double Lon { get; set; }
        public double Lat { get; set; }
        public int Capacity { get; set; }
    }
}