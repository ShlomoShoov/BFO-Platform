using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestApiService.Models.DTOs.RestDTOs;
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
        public async Task<ActionResult<IEnumerable<StationInformationAndStatus>>> GetStationInformationAndStatusesAsync(int? minAvailableBikes, bool? isRenting, bool? isReturning)
        {
            return Ok(await _repository.GetStationInformationAndStatusesAsync( minAvailableBikes,  isRenting, isReturning));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<StationInformationAndStatus>> GetStationInformationAndStatusesByIdAsync(string id)
        {
            StationInformationAndStatus? stationInformation = await _repository.GetStationInformationAndStatusesByIdAsync(id);
            if (stationInformation == null)
            {
                return NotFound();
            }
            return Ok(stationInformation);
        }
        [HttpGet("{id}/status")]
        public async Task<ActionResult<StationStatusOnlyDTO?>> GetStationStatusAsync(string id)
        {
            StationStatusOnlyDTO? result = await _repository.GetStationStatusAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpGet("{id}/history")]
        public async Task<ActionResult<IEnumerable<StationHistoryDTO>>> GetHistoryByIdAsync(string id, DateTime? from, DateTime? to, int? limit)
        {
            return Ok(await _repository.GetHistoryByIdAsync(id,from, to, limit));
        }
        [HttpGet("/api/dashboard")]
        public async Task<ActionResult<DashboardDTO>> GetDashboardAsync()
        {
            return Ok(await _repository.GetDashboardAsync());
        }


    }
}