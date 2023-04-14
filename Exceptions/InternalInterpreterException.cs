using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI
{
    public class InternalInterpreterException : Exception
    {
        public InternalInterpreterException()
        {
        }

        public InternalInterpreterException(string message)
            : base(message)
        {
        }

        public InternalInterpreterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
