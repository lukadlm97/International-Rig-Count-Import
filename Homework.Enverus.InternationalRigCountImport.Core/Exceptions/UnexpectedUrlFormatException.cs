using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework.Enverus.InternationalRigCountImport.Core.Exceptions
{
    public class UnexpectedUrlFormatException : Exception
    {
        public UnexpectedUrlFormatException()
        {
        }

        public UnexpectedUrlFormatException(string message)
            : base(message)
        {
        }

        public UnexpectedUrlFormatException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
