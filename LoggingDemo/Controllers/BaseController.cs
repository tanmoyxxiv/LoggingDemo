using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace LoggingDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly ILogger<BaseController> _logger;
        protected readonly IConfiguration _config;


        public BaseController(ILogger<BaseController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        protected void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }
        protected void LogError(string message)
        {
            _logger.LogError(message);
        }
        protected void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        protected SqlConnection GetSqlConnection()
        {
            try
            {
                string connectionString = _config.GetConnectionString("MyDbConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("Connection string not found in appsettings.json.");
                }
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                throw new Exception("Error connecting to database.", ex);
            }
        }
    }
}
