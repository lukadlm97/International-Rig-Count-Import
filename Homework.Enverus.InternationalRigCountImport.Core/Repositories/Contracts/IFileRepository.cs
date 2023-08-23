namespace Homework.Enverus.InternationalRigCountImport.Core.Repositories.Contracts
{
    public interface IFileRepository
    {
        Task<bool> SaveFile(string path, byte[] bytes, CancellationToken cancellationToken = default);
        Task<bool> SaveFile(string path, string content, CancellationToken cancellationToken = default);
    }
}
