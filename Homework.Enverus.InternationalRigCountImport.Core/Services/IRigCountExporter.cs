using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework.Enverus.InternationalRigCountImport.Core.Services
{
    public interface IRigCountExporter
    {
        Task<bool> Write(IEnumerable<IEnumerable<string>> stats, CancellationToken  cancellationToken=default);
    }
}
