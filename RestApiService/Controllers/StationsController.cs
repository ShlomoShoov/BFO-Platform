using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestApiService.Repositories;

namespace RestApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StationsController : ControllerBase
    {
        private IStationsRepository _repository;
        public StationsController(IStationsRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<ActionResult> Test()
        {
            return Ok("hello!!");
        }
    }
}