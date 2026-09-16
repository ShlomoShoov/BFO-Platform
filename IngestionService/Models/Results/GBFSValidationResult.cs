using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IngestionService.Models.Results
{
    public class GBFSValidationResult
    {
        public bool IsSuccess { get; set; }
        public string Reason { get; set; } = string.Empty;

        public static GBFSValidationResult Success()
        {
            return new  GBFSValidationResult { IsSuccess  = true};
        }
        public static  GBFSValidationResult Failed(string reason)
        {
            return new GBFSValidationResult { IsSuccess = false , Reason = reason};
        }
    }
}