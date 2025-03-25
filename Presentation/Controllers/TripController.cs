using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace TamplateASP.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController(ITripService tripService) : ControllerBase
    {
        [HttpGet("highest-tip-location")]
        public async Task<IActionResult> GetPuLocationWithHighestAvgTipAmount()
        {
            try
            {
                var puLocationId = await tripService.GetPULocationWithHighestAvgTipAmountAsync();
                return Ok(new { PULocationID = puLocationId });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("top-100-longest-by-distance")]
        public async Task<IActionResult> GetTop100LongestFaresByDistance()
        {
            try
            {
                var trips = await tripService.GetTop100LongestFaresByDistanceAsync();
                return Ok(trips);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("top-100-longest-by-time")]
        public async Task<IActionResult> GetTop100LongestFaresByTime()
        {
            try
            {
                var trips = await tripService.GetTop100LongestFaresByTimeAsync();
                return Ok(trips);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTripsByPuLocationId([FromQuery] int puLocationId)
        {
            try
            {
                if (puLocationId <= 0)
                {
                    return BadRequest("Invalid PULocationID.");
                }

                var trips = await tripService.GetTripsByPULocationIDAsync(puLocationId);
                return Ok(trips);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}