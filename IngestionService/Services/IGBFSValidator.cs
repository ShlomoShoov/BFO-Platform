using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IngestionService.Models.DTOs;
using IngestionService.Models.Results;

namespace IngestionService.Services
{
    public interface IGBFSValidator
    {
        public GBFSValidationResult Validate(IDTO DTO);
    }
}