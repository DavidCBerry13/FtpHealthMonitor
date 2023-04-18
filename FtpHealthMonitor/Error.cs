using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FtpHealthMonitor
{
    public sealed class Error
    {

        public Error(int code, string message) 
        { 
            ErrorCode = code;
            Message = message;
        }

        public int ErrorCode { get; init;  }
        public string Message { get; init; }


        public override bool Equals(object obj)
        {
            Error other = obj as Error;

            if (other == null) return false;

            return other.ErrorCode == this.ErrorCode;
        }


        public override int GetHashCode()
        {
            return ErrorCode.GetHashCode();
        }
        
    }
}
