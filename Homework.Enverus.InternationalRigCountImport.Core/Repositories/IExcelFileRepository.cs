using Homework.Enverus.InternationalRigCountImport.Core.Models;

namespace Homework.Enverus.InternationalRigCountImport.Core.Repositories
{
    public interface IExcelFileRepository
    {
        Task<bool> SaveFile(byte[] file, string dateDirectory, string timeDirectory, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<IReadOnlyList<string>>>
            LoadFile(string dateDirectory, string timeDirectory, CancellationToken cancellationToken = default);
    }
}
