using Homework.Enverus.InternationalRigCountImport.Core.Models;
using Homework.Enverus.InternationalRigCountImport.Core.Models.DTOs;

namespace Homework.Enverus.InternationalRigCountImport.Core.Services.Contracts
{
    public interface IRigCountImporter
    {
        Task<OperationResult<FileDirectory>> Import(bool? advancedHandling = false,
            bool? useArchive = false,
            CancellationToken cancellationToken = default);
    }
}
