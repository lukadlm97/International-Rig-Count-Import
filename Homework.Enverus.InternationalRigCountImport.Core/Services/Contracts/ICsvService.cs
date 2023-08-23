

namespace Homework.Enverus.InternationalRigCountImport.Core.Services.Contracts
{
    public interface ICsvService
    {
        Task<bool> SaveFile(IReadOnlyList<IReadOnlyList<string>> rigRows,
            int? years = null,
            int? rowsPerYear = null,
            string? delimiter = null,
            bool? advancedHandling = null,
            string? csvLocation = null,
            CancellationToken cancellationToken = default);
    }
}
