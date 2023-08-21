using System.Text;
using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Repositories;
using Microsoft.Extensions.Options;

namespace Homework.Enverus.InternationalRigCountImport.Core.Services
{
    public class RigCountExporter:IRigCountExporter
    {
        private readonly CsvExporterSettings _csvExporterSettings;
        private readonly ICsvRepository _csvRepository;

        public RigCountExporter(IOptions<CsvExporterSettings> options, ICsvRepository csvRepository)
        {
            _csvExporterSettings = options.Value;
            _csvRepository = csvRepository;
        }
        public async Task<bool> Write(IEnumerable<IEnumerable<string>> stats, CancellationToken cancellationToken = default)
        {
            var rowCount = _csvExporterSettings.RowsPerYear * _csvExporterSettings.Years;

            if (stats.Count() < rowCount)
            {
                throw new Exception();
            }

            var selectedStats = stats.Take(rowCount);

            StringBuilder sb = new StringBuilder();

            foreach (var row in selectedStats)
            {
                sb.AppendLine(string.Join(_csvExporterSettings.Delimiter, row));
            }

            return await _csvRepository.SaveFile(sb.ToString(), _csvExporterSettings.FileName, cancellationToken);
        }
    }
}
