using Homework.Enverus.InternationalRigCountImport.Core.Models;

namespace Homework.Enverus.InternationalRigCountImport.Core.Repositories.Contracts
{
    public interface IExcelFileRepository
    {
        Task<bool> SaveFile(byte[] file, 
            bool advancedHandling= false, 
            string dateDirectory = default, 
            string timeDirectory = default, 
            CancellationToken cancellationToken = default);
        
        Task<IReadOnlyList<IReadOnlyList<string>>>
            LoadFile(bool advancedHandling = false, 
                string dateDirectory = default, 
                string timeDirectory = default, 
                CancellationToken cancellationToken = default);
    }
}
