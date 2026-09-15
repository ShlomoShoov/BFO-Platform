using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IngestionService.Models.Configurations
{
    public class ServicesConfiguration
    {
        public int StationInformationServiceSecondsTrigger { get; set; }
        public int StationStatusServiceSecondsTrigger { get; set; }

        public int VehicleTypesServiceSecondsTrigger { get; set; }

    }
}