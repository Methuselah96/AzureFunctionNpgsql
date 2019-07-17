using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Text;

namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var result = new StringBuilder();

            using (var connection = new NpgsqlConnection(Environment.GetEnvironmentVariable("PostgresConnectionString")))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand("SELECT \"Guid\" FROM \"Settings\"", connection))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        result.AppendLine(reader.GetGuid(0).ToString());
            }

            return new OkObjectResult(result.ToString());
        }
    }
}
