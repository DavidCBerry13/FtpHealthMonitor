using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpHealthMonitor
{
    public class Result
    {

        /// <summary>
        /// Indicates if the operation was successful or not
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// Property containing an Error object if the operation failed
        /// </summary>
        public Error Error { get; private set; }


        /// <summary>
        /// Creates a new result object.  This constructor is protected to insure
        /// Result objects are only created by their static factory methods
        /// </summary>
        /// <param name="success">A bool indicating the success of failure of the call</param>
        /// <param name="error">An Error object representing an error that occurred.  Should be null for successful operations</param>
        protected Result(bool success, Error error)
        {
            IsSuccess = success;
            Error = error;
        }


        /// <summary>
        /// Creates a successful Result object that does not return any data
        /// </summary>
        /// <returns></returns>
        public static Result Success()
        {
            return new Result(true, null);
        }

        public static Result Failure(Error error)
        {
            return new Result(false, error);
        }
    }
}
