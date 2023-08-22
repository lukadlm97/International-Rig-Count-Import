using Homework.Enverus.InternationalRigCountImport.Core.Models;
using Homework.Enverus.InternationalRigCountImport.Core.Models.DTOs;

namespace Homework.Enverus.InternationalRigCountImport.Core.Fetchers.Contracts
{
    public interface IFileProvider
    {
        Task<OperationResult<RawFile>> 
            GetInternationalRigCount(string? dynamicUrl=default, CancellationToken cancellationToken=default);
    }
}
