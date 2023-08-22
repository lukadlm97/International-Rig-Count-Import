using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework.Enverus.InternationalRigCountImport.Core.Repositories.Contracts
{
    public interface ICsvRepository
    {
        Task<bool> SaveFile(string content, string fileName, bool advancedHandling=false,string path=null,CancellationToken cancellationToken = default);
    }
}
