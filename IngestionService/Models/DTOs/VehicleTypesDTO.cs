using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IngestionService.Models.DTOs
{
    public class VehicleTypesDTO
    {
        public string VehicleTypeId { get; set; } = string.Empty;
        public string FormFactor { get; set; } = string.Empty;
        public string PropulsionType { get; set; } = string.Empty;
    }
}