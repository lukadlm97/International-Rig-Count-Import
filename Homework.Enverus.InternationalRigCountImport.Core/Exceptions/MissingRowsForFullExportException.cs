using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework.Enverus.InternationalRigCountImport.Core.Exceptions
{
    public class MissingRowsForFullExportException : Exception
    {
        public MissingRowsForFullExportException()
        {
        }

        public MissingRowsForFullExportException(string message)
            : base(message)
        {
        }

        public MissingRowsForFullExportException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
