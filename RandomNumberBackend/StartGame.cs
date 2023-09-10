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

namespace RandomNumberBackend
{
    public static class StartGame
    {
        [FunctionName("StartGame")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string nickname = req.Query["nickname"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            nickname = nickname ?? data?.nickname;

            if (string.IsNullOrEmpty(nickname))
            {
                return new BadRequestResult();
            }

            int hiddenNumber = 45;

            string responseMessage = $"{nickname} your hidden number is {hiddenNumber}.";
            return new OkObjectResult(responseMessage);
        }
    }
}
