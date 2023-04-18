using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpHealthMonitor
{
    public class FtpFailureResult : ObjectResult
    {
        public FtpFailureResult(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }



    }
}
