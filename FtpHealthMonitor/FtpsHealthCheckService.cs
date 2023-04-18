using FluentFTP;
using FluentFTP.Exceptions;
using FluentFTP.Helpers;
using Renci.SshNet;
using SshNet.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FtpHealthMonitor.Errors;
using static System.Net.WebRequestMethods;

namespace FtpHealthMonitor
{
    public class FtpsHealthCheckService
    {





        public Result TestConnection(string ftpsSererName, string ftpsUsername, string ftpsPassword, string ftpsFilePath, byte[] fileBytes)
        {

            using (var ftpsClient = new FtpClient(ftpsSererName, ftpsUsername, ftpsPassword))
            {
                try
                {
                    ftpsClient.Config.EncryptionMode = FtpEncryptionMode.Explicit;
                    ftpsClient.Config.ValidateAnyCertificate = true;
                    ftpsClient.Config.ConnectTimeout = 20000;
                    ftpsClient.Connect();
                }
                catch (TimeoutException timeoutException)
                {
                    return Result.Failure(Errors.FtpServerConnectionTimeout.Create(ftpsSererName));

                }
                catch (FtpAuthenticationException authException) // Login Exception
                {
                    return Result.Failure(Errors.FtpServerLoginFailed.Create(ftpsSererName, ftpsUsername));
                }
                catch (Exception exception)
                {
                    // Some other exception during the connect process
                    return Result.Failure(Errors.OtherError.Create(exception.Message));
                }

                // Now try to upload a file
                try
                {
                    ftpsClient.UploadBytes(fileBytes, ftpsFilePath, FtpRemoteExists.Overwrite);
                }
                catch (Exception exception)
                {
                    return Result.Failure(Errors.FtpFileUploadFailed.Create(ftpsSererName, exception.Message));
                }

                // Download that same file
                try
                {
                        using (MemoryStream downloadStream = new MemoryStream())
                        {
                            ftpsClient.DownloadStream(downloadStream, ftpsFilePath);
                            // TODO: Compute the hash and make sure it matches the original
                        }                   
                }
                catch (Exception exception)
                {
                    return Result.Failure(Errors.FtpFileDownloadFailed.Create(ftpsSererName, exception.Message));
                }

                // Delete the file
                try
                {
                    ftpsClient.DeleteFile(ftpsFilePath);
                }
                catch (Exception exception)
                {
                    return Result.Failure(Errors.OtherError.Create("Failure deleting file - " + exception.Message));
                }

                // Disconnect
                ftpsClient.Disconnect();


                return Result.Success();
            }


        }
    }
}
