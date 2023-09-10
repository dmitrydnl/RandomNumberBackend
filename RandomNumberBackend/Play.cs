using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace RandomNumberBackend
{
    public static class Play
    {
        [FunctionName("Play")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string nickname = req.Query["nickname"];
            string myNumberRaw = req.Query["myNumber"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            nickname = nickname ?? data?.nickname;
            myNumberRaw = myNumberRaw ?? data?.myNumber;

            if (string.IsNullOrEmpty(nickname))
            {
                return new BadRequestObjectResult($"nickname is empty");
            }

            if (string.IsNullOrEmpty(myNumberRaw) || !Int32.TryParse(myNumberRaw, out int myNumber))
            {
                return new BadRequestObjectResult($"myNumber is empty or not a number");

            }

            return new OkObjectResult($"{nickname} your number is {myNumber}.");
        }
    }
}
