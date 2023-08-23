namespace Homework.Enverus.InternationalRigCountImport.Core.Services.Contracts
{
    public interface IExcelService
    {
        Task<bool> SaveFile(byte[] file,
            bool? advancedHandling = null,
            bool? useArchive = null,
            string? dateDirectory = null,
            string? timeDirectory = null,
            CancellationToken cancellationToken = default);

        IReadOnlyList<IReadOnlyList<string>>
            LoadFile(bool advancedHandling = false,
                string? dateDirectory = default,
                string? timeDirectory = default);
    }
}
