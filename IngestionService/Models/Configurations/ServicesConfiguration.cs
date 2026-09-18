using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IngestionService.Models.Configurations
{
    public class ServicesConfiguration
    {
        public int StationInformationServiceMinuetsTrigger { get; set; }
        public int StationStatusServiceMinuetsTrigger { get; set; }

        public int VehicleTypesServiceMinuetsTrigger { get; set; }

    }
}