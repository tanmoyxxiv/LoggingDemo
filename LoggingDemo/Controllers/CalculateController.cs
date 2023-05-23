using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LoggingDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculateController : ControllerBase
    {
        private readonly ILogger<CalculateController> _logger;

        public CalculateController(ILogger<CalculateController> logger)
        {
            _logger = logger;
        }


        [HttpGet("/divide")]
        public IActionResult GetDivisionResult(int? a, int? b)
        {
            _logger.LogInformation("Requested GetDivisionResult()");

            if (!a.HasValue || !b.HasValue)
            {
                _logger.LogWarning("Incomplete Input for Division Operation");
                return BadRequest("Please enter value for both 'a' and 'b' : ");
            }
            try
            {
                int c = (int)(a / b);
                var result = a + "/" + b + " = " + c;

                _logger.LogInformation("GetDivisionResult() Request Successful with value {@Result}", result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }
    }
}
