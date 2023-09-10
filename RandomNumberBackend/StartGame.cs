using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Security.Cryptography;
using RandomNumberBackend.Database;
using RandomNumberBackend.Game;

namespace RandomNumberBackend
{
    public class StartGame
    {
        private readonly IDatabase database;
        private readonly INumberGenerator numberGenerator;

        public StartGame(IDatabase database, INumberGenerator numberGenerator)
        {
            this.database = database;
            this.numberGenerator = numberGenerator;
        }

        [FunctionName("StartGame")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
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

            int hiddenNumber = numberGenerator.Generate();

            bool result = database.CreateGame(nickname, hiddenNumber);
            if (!result)
            {
                return new BadRequestObjectResult("game already exist");
            }

            return new OkObjectResult("game started");
        }
    }
}
