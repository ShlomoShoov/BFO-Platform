using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IngestionService.Models.Configurations
{
    public class StationsAPIs
    {
        public string StationInformationAPI { get; set; } = string.Empty;
        public List<string> StationInformationKeyPath {get; set;}= [] ;
        public string StationStatusAPI { get; set; } = string.Empty;
        public List<string> StationStatusKeyPath { get; set; } = [];
        public string VehicleTypesAPI { get; set; } = string.Empty;
        public List<string> VehicleTypesKeyPath { get; set; } = [];


    }
}