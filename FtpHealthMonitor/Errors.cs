using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpHealthMonitor
{
    public static class Errors
    {

        public static class HealthCheckFunctionConfigError
        {
            public static readonly int Code = 581;

            public static Error Create(string message) => new Error(Code, message);
        }


        public static class FtpServerConnectionTimeout
        {
            public static readonly int Code = 582;

            public static readonly string Message = "Unable to connect to FTP Server {0} - Connection timed out";

            public static Error Create(string ftpServerName) => new Error(Code, string.Format(Message, ftpServerName));
        }


        public static class FtpServerLoginFailed
        {
            public static readonly int Code = 583;

            public static readonly string Message = "Login failed for FTP Server {0} with username {1}";

            public static Error Create(string ftpServerName, string ftpUsername) => new Error(Code, string.Format(Message, ftpServerName, ftpUsername));
        }



        public static class FtpFileUploadFailed
        {
            public static readonly int Code = 585;

            public static readonly string Message = "Unable to upload file to FTP Server {0} - Error {1}";

            public static Error Create(string ftpServerName, string errorMessage) => new Error(Code, string.Format(Message, ftpServerName, errorMessage));
        }


        public static class FtpFileDownloadFailed
        {
            public static readonly int Code = 586;

            public static readonly string Message = "Unable to download file to FTP Server {0} - Error {1}";

            public static Error Create(string ftpServerName, string errorMessage) => new Error(Code, string.Format(Message, ftpServerName, errorMessage));
        }


        public static class FtpFileChecksumFailed
        {
            public static readonly int Code = 587;

            public static readonly string Message = "File checksum does not match between original and downloaded files (FTP Server {0})";

            public static Error Create(string ftpServerName) => new Error(Code, string.Format(Message, ftpServerName));
        }


        public static class FileTransferTooSlow
        {
            public static readonly int Code = 588;

            public static readonly string Message = "File transfers succeeded, but time to complete transfers was outside of acceptable rages. Server {0} - Upload time {1} ms - Download time {2} ms ";

            public static Error Create(string ftpServerName, int uploadMilliseconds, int downloadMilliseconds)
                => new Error(Code, string.Format(Message, ftpServerName));
        }


        public static class OtherError
        {
            public static readonly int Code = 589;

            public static Error Create(string message) => new Error(Code, message);
        }

        //public static readonly string CheckFunctionConfigError = "581";

        //public static readonly string FtpServerConnectionTimeout = "582";

        //public static readonly string FtpServerLoginFailed = "583";

        public static readonly string FtpServerInvalidCertificate = "584";

        //public static readonly string FtpFileUploadFailed = "585";

        //public static readonly string FtpFileDownloadFailed = "586";

        //public static readonly string FtpFileChecksumFailed = "587";

        //public static readonly string FileTransferTooSlow = "588";

        //public static readonly string OtherError = "599";


    }
}
