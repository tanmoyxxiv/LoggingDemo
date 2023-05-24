using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace LoggingDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculateController : BaseController
    {
        public CalculateController(ILogger<BaseController> logger, IConfiguration config) : base(logger,config)
        {
        }


        [HttpGet("/divide")]
        public IActionResult GetDivisionResult(int? a, int? b)
        {
            LogInformation("Requested GetDivisionResult()");

            if (!a.HasValue || !b.HasValue)
            {
                LogWarning("Incomplete Input for Division Operation");
                return BadRequest("Please enter value for both 'a' and 'b' : ");
            }
            try
            {
                int c = (int)(a / b);
                var result = a + "/" + b + " = " + c;

                LogInformation("GetDivisionResult() Request Successful with value {@Result} " + result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            using (SqlConnection connection = GetSqlConnection())
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM MyTable WHERE Id = {id}", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string result = reader.GetString(1) + " " + reader.GetString(2); 
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}
