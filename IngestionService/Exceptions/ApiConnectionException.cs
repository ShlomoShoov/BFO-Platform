using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IngestionService.Exceptions
{
    public class ApiConnectionException: Exception
    {
        public string Url { get; set; }

        public ApiConnectionException(string url, string message) : base(message)
        {
            Url = url;
        }
    }
}