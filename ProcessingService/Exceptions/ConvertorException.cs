using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessingService.Exceptions
{
    public class ConvertorException :Exception
    {
        public ConvertorException(string message):base(message){}
    }
}