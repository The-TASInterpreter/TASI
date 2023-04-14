using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI.Exceptions
{
    public class CodeSyntaxException : Exception
    {
        public CodeSyntaxException()
        {
        }

        public CodeSyntaxException(string message)
            : base(message)
        {
        }

        public CodeSyntaxException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
