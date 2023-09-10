using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RandomNumberBackend.Database;

namespace RandomNumberBackend
{
    public class UserStatistics
    {
        private readonly IDatabase database;

        public UserStatistics(IDatabase database)
        {
            this.database = database;
        }

        [FunctionName("UserStatistics")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string nickname = req.Query["nickname"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            nickname = nickname ?? data?.nickname;

            if (string.IsNullOrEmpty(nickname))
            {
                return new BadRequestObjectResult("nickname is empty");
            }

            var statistics = database.GetUserStatistics(nickname);
            return new OkObjectResult(statistics);
        }
    }
}

