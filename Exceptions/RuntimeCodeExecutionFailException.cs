using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI.Exceptions
{
    public class RuntimeCodeExecutionFailException : Exception
    {
        public string exceptionType = String.Empty;
        public RuntimeCodeExecutionFailException()
        {
        }

        public RuntimeCodeExecutionFailException(string message, string exceptionType)
            : base(message)
        {
            this.exceptionType = exceptionType;
        }

        public RuntimeCodeExecutionFailException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
