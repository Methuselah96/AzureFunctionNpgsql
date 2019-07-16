using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace FunctionApp1
{
    public static class Function1
    {
        private const string FindModelsWithSettingsQuery = "select \"Guid\" from \"Settings\" where \"Guid\" = any(@modelGuids)";

        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            IDbConnection connection = new NpgsqlConnection(Environment.GetEnvironmentVariable("PostgresConnectionString"));

            var modelGuids = new List<Guid> { Guid.Empty };
            var result = await connection.QueryAsync<Guid>(FindModelsWithSettingsQuery, new { modelGuids });

            return result != null
                ? (ActionResult)new OkObjectResult(result)
                : new BadRequestObjectResult("Response is null.");
        }
    }
}
