using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace LoggingDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculateController : BaseController
    {
        //private readonly string serverName = "LTIN388889\\SQLEXPRESS";
        private readonly string dbName = "LoggingDb";

        public CalculateController(): base()
        {

        }

        [HttpGet("/divide")]
        public IActionResult GetDivisionResult(int? a, int? b)
        {

            var lineNo = 2;
            string fileName = "sample.cs";
            string token = "slkjdfsdjk";
            Log.Information("Requested GetDivisionResult()");

            LogInformation("Message saved"," Announcement saved successfully ", lineNo, fileName, token);
            
            if (!a.HasValue || !b.HasValue)
            {
                LogWarning("Incomplete Input for Division Operation");
                return BadRequest("Please enter value for both 'a' and 'b' : ");
            }
            try
            {
                int c = (int)(a / b);
                var result = a + "/" + b + " = " + c;

                Log.Information("GetDivisionResult() Request Successful with value " + result);

                return Ok(result);
            }
            catch (Exception ex)
            {
                LogError("Error occurrued", ex.Message, lineNo, fileName, token);
                Log.Error(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            using (SqlConnection connection = GetSqlConnection(dbName))
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM MyTable WHERE Id = {id}", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string result = reader.GetString(1) + " " + reader.GetString(2);
                    Log.Information(result);
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
        }
        
        [HttpGet]
        public ActionResult<string> Get()
        {
            using (SqlConnection connection = GetSqlConnection(dbName))
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM MyTable WHERE Id = 1", connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string result = reader.GetString(1) + " " + reader.GetString(2);
                    Log.Information(result);
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
