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
    public class Authorization
    {
        private readonly IDatabase database;

        public Authorization(IDatabase database)
        {
            this.database = database;
        }

        [FunctionName("Authorization")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string nickname = req.Query["nickname"];
            string password = req.Query["password"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            nickname = nickname ?? data?.nickname;
            password = password ?? data?.password;

            if (string.IsNullOrEmpty(nickname))
            {
                return new BadRequestObjectResult("nickname is empty");
            }

            if (string.IsNullOrEmpty(password))
            {
                return new BadRequestObjectResult("password is empty");
            }

            if (!database.Authorization(nickname, password))
            {
                return new BadRequestObjectResult("nickname or password is wrong");
            }

            return new OkObjectResult("Authorization successful");
        }
    }
}
