using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using ProcessingService.Models.DTOs;

namespace ProcessingService.Services
{
    public interface IEventsConvertor
    {
        public IDTO GetDtoFromConsumeResult(ConsumeResult<Null, string> consumeResult);
    }
}