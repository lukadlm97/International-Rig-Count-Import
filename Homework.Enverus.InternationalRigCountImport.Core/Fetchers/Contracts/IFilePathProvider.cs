using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Contracts
{
    public interface IFilePathProvider
    {
        Task<string?> GetFilePath(CancellationToken  cancellationToken);
    }
}
