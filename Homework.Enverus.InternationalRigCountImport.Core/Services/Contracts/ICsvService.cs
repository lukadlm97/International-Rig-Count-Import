

namespace Homework.Enverus.InternationalRigCountImport.Core.Services.Contracts
{
    public interface ICsvService
    {
        Task<bool> SaveFile(IReadOnlyList<IReadOnlyList<string>> rigRows,
            int? years = null,
            int? rowsPerYear = null,
            string? delimiter = null,
            CancellationToken cancellationToken = default);
    }
}
