using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IngestionService.Exceptions
{
    public class ApiResponseException :Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; }
        public ApiResponseException(HttpStatusCode statusCode, string content) : base()
        {
            StatusCode = statusCode;
            Content = content;
        }
    }
}