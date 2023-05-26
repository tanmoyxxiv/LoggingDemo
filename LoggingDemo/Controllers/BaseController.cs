using Microsoft.AspNetCore.Mvc;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog;
using System.Data.SqlClient;
using System.Text.Json.Serialization;

namespace LoggingDemo.Controllers
{

    public class BaseController : ControllerBase
    {
        //Stores the name of the server where the database is hosted
        private static string serverName = "LTIN388889\\SQLEXPRESS";

        //Constructor of the class configures the Serilog logger by setting the minimum log level,
        //defining the output to the console and file, and creating a logger instance.
        public BaseController()
        {
            //Serilog Configuration
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.Console(new JsonFormatter())
                .WriteTo.File("./Logs/log.txt", shared:true, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            //Confirmation of logging 
            Log.Information("Logger configured in BaseController");

        }

        //Utility method for logging information using the Serilog logger.
        protected void LogInformation(string message1, string message2, int lineNo, string fileName, string token)
        {
            Log.Information("{Message1}, {Message2}, LineNo : {LineNo}, FileName : {FileName}, Token : {Token} ", message1, message2, lineNo, fileName, token);
        }

        //Utility method for logging error using the Serilog logger.
        protected void LogError(string message, string exceptionMessage, int lineNo, string fileName, string token)
        {
            Log.Error("{Message}, {ExceptionMessage} , LineNo : {LineNo}, FileName : {FileName}, Token : {Token} ", message, exceptionMessage, lineNo, fileName, token);
        }

        //Utility method for logging warning using the Serilog logger.
        protected void LogWarning(string message)
        {
            Log.Warning(message);
        }

        //Establishes a database connection to the specified database using the server name and returns a SqlConnection object.
        protected SqlConnection GetSqlConnection(string dbName)
        {
            try
            {
                //Creation of Connection String
                string connectionString = "Server=" + serverName + ";Database=" + dbName + ";Trusted_Connection=True;";

                //checks if connection string empty
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("Connection string is empty");
                }

                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                //returns the required SqlConnection Object
                return connection;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                throw new Exception("Error connecting to database.", ex);
            }
        }

        //Generic class that defines the structure of the response returned by the API endpoints.
        public class Response<T>
        {
            [JsonPropertyName("statusCode")]
            public int StatusCode { get; set; } 
            [JsonPropertyName("statusMessage")]
            public string StatusMessage { get; set; }
            [JsonPropertyName("correlationId")]
            public Guid CorrelationId { get; set; }
            [JsonPropertyName("data")]
            public T Data { get; set; }
            [JsonPropertyName("errorMessage")]
            public string ErrorMessage  { get; set; }   
        }
    }
}
