using Homework.Enverus.InternationalRigCountImport.Core.Models;
using Homework.Enverus.InternationalRigCountImport.Core.Models.DTOs;

namespace Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Contracts
{
    public interface IWebPageProcessor
    {
        Task<OperationResult<RawFile>> 
            GetInternationalRigCount(CancellationToken cancellationToken);
    }
}
