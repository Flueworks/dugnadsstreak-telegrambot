using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

// 1001583926:AAGmhMtVdEPFCnES9FRsN6xB_GClkwt7LbI

namespace BKM.Dugnad
{
    public static class TelegramMessageReceived
    {
        [FunctionName("TelegramMessageReceived")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [Queue("incoming", Connection = "AzureWebJobsStorage")]out string output,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var message = req.ReadAsStringAsync().GetAwaiter().GetResult();

            output = message;

            return new OkResult();
        }
    }
}
