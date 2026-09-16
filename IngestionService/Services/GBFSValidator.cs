using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IngestionService.Models.DTOs;
using IngestionService.Models.Results;

namespace IngestionService.Services
{
    public class GBFSValidator : IGBFSValidator
    {
        public GBFSValidationResult Validate(IDTO DTO)
        {
            return DTO switch
            {
                StationInformationDTO dto => Validate(dto),
                StationStatusDTO dto => Validate(dto),
                VehicleTypesDTO dto => Validate(dto),
                _ => throw new NotImplementedException()
            };
        }
        public GBFSValidationResult Validate(StationInformationDTO stationInformation)
        {
            if (string.IsNullOrWhiteSpace(stationInformation.StationId))
            {
                return GBFSValidationResult.Failed("Id must contain value but got Null");
            }
            if (stationInformation.Lat < -90 || stationInformation.Lat > 90)
            {
                return GBFSValidationResult.Failed($"Lat must be between -90 to 90 but got {stationInformation.Lat}");
            }
            if (stationInformation.Lon < -180 || stationInformation.Lon > 180)
            {
                return GBFSValidationResult.Failed($"Lon must be between -180 to 180 but got {stationInformation.Lon}");
            }
            if (_IsNegative(stationInformation.Capacity))
            {
                return GBFSValidationResult.Failed($"Capacity must be positive, Got {stationInformation.Capacity}");
            } 
            
            return GBFSValidationResult.Success();
        }
        
        public GBFSValidationResult Validate(StationStatusDTO stationStatus)
        {
            if (string.IsNullOrWhiteSpace(stationStatus.StationId))
            {
                return GBFSValidationResult.Failed("Id must contain value but got Null");
            }
            if (_IsNegative(stationStatus.NumBikesAvailable))
            {
                return GBFSValidationResult.Failed($"Bikes available must be positive, but got {stationStatus.NumBikesAvailable}");
            }
            if (_IsNegative(stationStatus.NumDocksAvailable))
            {
                return GBFSValidationResult.Failed($"Dock available must be positive, but got {stationStatus.NumDocksAvailable}");
            }
            return GBFSValidationResult.Success();

        }
        public GBFSValidationResult Validate(VehicleTypesDTO vehicleType )
        {
            return GBFSValidationResult.Success();
        }

        private bool _IsNegative(int number)
        {
            return number < 0;
        }


    }
}