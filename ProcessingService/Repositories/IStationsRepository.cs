using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProcessingService.Models.DTOs;

namespace ProcessingService.Repositories
{
    public interface IStationsRepository
    {
        public  Task Init();

        public Task AddAsync(IDTO DTO);

    }
}