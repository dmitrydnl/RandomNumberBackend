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
using RandomNumberBackend.Game;

namespace RandomNumberBackend
{
    public class Play
    {
        private readonly IDatabase database;
        private readonly INumberValidator numberValidator;

        public Play(IDatabase database, INumberValidator numberValidator)
        {
            this.database = database;
            this.numberValidator = numberValidator;
        }

        [FunctionName("Play")]
        public async Task<IActionResult> Run(
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
                return new BadRequestObjectResult("nickname is empty");
            }

            if (string.IsNullOrEmpty(myNumberRaw) || !Int32.TryParse(myNumberRaw, out int myNumber))
            {
                return new BadRequestObjectResult("myNumber is empty or not a number");

            }

            var hiddenNumber = database.GetCurrentGame(nickname);
            if (hiddenNumber is null)
            {
                return new BadRequestObjectResult("Current game not exist");
            }

            var status = numberValidator.Validate(myNumber, hiddenNumber.Value);
            switch (status)
            {
                case NumberValidatorStatus.Greater:
                    return new BadRequestObjectResult("Your number is greater");
                case NumberValidatorStatus.Less:
                    return new BadRequestObjectResult("Your number is less");
            }

            database.FinishGame(nickname);

            return new OkObjectResult("Your number is equal, game finished");
        }
    }
}
