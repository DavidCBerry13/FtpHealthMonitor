using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FtpHealthMonitor
{
    /// <summary>
    /// SImple function to check and make sure you can access the key vault
    /// </summary>
    public static class KeyVaultAccessCheck
    {
        [FunctionName("KeyVaultAccessCheck")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string fromKeyVault = Environment.GetEnvironmentVariable("MySetting");

            string responseMessage = $"The value is {fromKeyVault}";

            return new OkObjectResult(responseMessage);
        }
    }
}
