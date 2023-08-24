using Homework.Enverus.InternationalRigCountImport.Core.Models;
using Homework.Enverus.InternationalRigCountImport.Core.Models.DTOs;

namespace Homework.Enverus.InternationalRigCountImport.Core.Services.Contracts
{
    public interface IRigCountExporter
    {
        Task<OperationResult<ExportFileDirectory>> Export(int? year,
                                                            int? rowPerYear,
                                                            string? delimiter = null,
                                                            string? dateDir = null,
                                                            string? timeDir = null,
                                                            CancellationToken cancellationToken = default);
    }
}
