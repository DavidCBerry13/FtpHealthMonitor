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
    public static class FtpsHealthCheck
    {
        [FunctionName("FtpsHealthCheck")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string ftpServerName = Environment.GetEnvironmentVariable("FtpServiceName", EnvironmentVariableTarget.Process);
            string ftpUsername = Environment.GetEnvironmentVariable("FtpUserName", EnvironmentVariableTarget.Process);
            string ftpPassword = Environment.GetEnvironmentVariable("FtpPassword", EnvironmentVariableTarget.Process);
            string ftpFilePath = Environment.GetEnvironmentVariable("FtpFilePath", EnvironmentVariableTarget.Process);

            byte[] fileBytes = new byte[] { };


            // TODO: Check the config
            


            FtpsHealthCheckService healthCheckService = new FtpsHealthCheckService();
            var result = healthCheckService.TestConnection(ftpServerName, ftpUsername, ftpPassword, ftpFilePath, fileBytes);

            if (result.IsSuccess)
                return new OkResult();
            else
                return new FtpFailureResult(result.Error.ErrorCode, result.Error.Message);


        }
    }
}
