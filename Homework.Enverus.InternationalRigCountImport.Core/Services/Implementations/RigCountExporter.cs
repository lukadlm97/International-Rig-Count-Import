using System.Text;
using Homework.Enverus.InternationalRigCountImport.Core.Configurations;
using Homework.Enverus.InternationalRigCountImport.Core.Exceptions;
using Homework.Enverus.InternationalRigCountImport.Core.Repositories.Contracts;
using Homework.Enverus.InternationalRigCountImport.Core.Services.Contracts;
using Microsoft.Extensions.Options;

namespace Homework.Enverus.InternationalRigCountImport.Core.Services
{
    public class RigCountExporter:IRigCountExporter
    {
        private readonly Exporter _exporterSettings;
        private readonly ICsvRepository _csvRepository;
        private readonly AdvancedSettings _advancedSettings;

        public RigCountExporter(IOptions<Exporter> options, ICsvRepository csvRepository, IOptions<AdvancedSettings> advancedOptions)
        {
            _exporterSettings = options.Value;
            _csvRepository = csvRepository;
            _advancedSettings = advancedOptions.Value;
        }
        public async Task<bool> Write(IEnumerable<IEnumerable<string>> stats, CancellationToken cancellationToken = default)
        {
            var rowCount = _exporterSettings.DataSourceSettings.ExcelWorkbookSettings.Years *
                           _exporterSettings.DataSourceSettings.ExcelWorkbookSettings.RowsPerYear;

            if (stats.Count() < rowCount)
            {
                throw new MissingRowsForFullExportException();
            }

            var selectedStats = stats.Take(rowCount);

            StringBuilder sb = new StringBuilder();

            foreach (var row in selectedStats)
            {
                sb.AppendLine(string.Join(_exporterSettings?.ExportDestinationSettings?.CsvSettings?.Delimiter, row));
            }

            if (_advancedSettings.Enabled)
            {
                return await _csvRepository.SaveFile(sb.ToString(), _exporterSettings.ExportDestinationSettings.CsvSettings.FileName, true, _advancedSettings.CsvExportLocation, cancellationToken);
            }
            return await _csvRepository.SaveFile(sb.ToString(), _exporterSettings.ExportDestinationSettings.CsvSettings.FileName, false, string.Empty, cancellationToken);
        }
    }
}
