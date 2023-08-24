namespace Homework.Enverus.InternationalRigCountImport.CAF.Scripts.Contracts
{
    public interface IScript
    {
        Task Import([Option("advancedHandling")] bool? advancedHandling = null,
            [Option("useArchive")] bool? useArchive = null, 
            [Option("year")] int? year = null, 
            [Option("rowPerYear")] int? rowPerYear = null, 
            [Option("delimiter")] string? delimiter = null,
            CancellationToken cancellationToken = default);
    }
}
