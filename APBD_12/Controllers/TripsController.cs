using APBD_12.Models.DTOs;
using APBD_12.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_12.Controllers
{
    [ApiController]
    [Route("api/trips")]
    public class TripsController : ControllerBase
    {
        private readonly ITripsService _tripService;

        public TripsController(ITripsService tripsService)
        {
            _tripService = tripsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _tripService.GetTripsAsync(page, pageSize);
            return Ok(result);
        }
        
        [HttpGet("test")]
        public IActionResult Test() => Ok("API is working!");
        
        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] ClientAssignedTripDTO dto)
        {
            try
            {
                await _tripService.AssignClientToTripAsync(idTrip, dto);
                return Ok("Client assigned to trip successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }
        

    }
}
