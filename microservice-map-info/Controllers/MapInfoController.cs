using GoogleMapInfo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace microservice_map_info.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapInfoController : ControllerBase
    {
        private readonly GoogleDistanceApi _googleDistanceApi;
        private readonly ILogger<MapInfoController> _logger;

        public MapInfoController(GoogleDistanceApi googleDistanceApi, ILogger<MapInfoController> logger)
        {
            _googleDistanceApi = googleDistanceApi;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogInformation("Starting Map Info Controller.");
        }
        [HttpGet]
        public async Task<ActionResult<GoogleDistanceData>> GetDistance(string originCity,
        string destinationCity)
        {
            try
            {
                return await _googleDistanceApi.GetMapDistance(originCity, destinationCity);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"Error getting map distance: ${originCity} to ${destinationCity}, status code: {ex.StatusCode}");
                return StatusCode(500);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting map distance: ${originCity} to ${destinationCity}");
                return StatusCode(500);
            }
        }
    }
}
