using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RandomNumberBackend.Database;

namespace RandomNumberBackend
{
    public class GlobalStatistics
    {
        private readonly IDatabase database;

        public GlobalStatistics(IDatabase database)
        {
            this.database = database;
        }

        [FunctionName("GlobalStatistics")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            KeyValuePair<string, List<int>>[] statistics = database.GetGlobalStatistics();
            return new OkObjectResult(statistics);
        }
    }
}
