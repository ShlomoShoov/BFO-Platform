using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IngestionService.Exceptions
{
    public class DeserializeException : Exception
    {
        public string RawText { get; set; }
        public string ObjectName { get; set; }

        public DeserializeException(string rawText, string objectName, string error) : base(error)
        {
            RawText = rawText;
            ObjectName = objectName;
        }
    }
}