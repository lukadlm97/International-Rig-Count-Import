using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework.Enverus.InternationalRigCountImport.Core.Configurations
{
    public class AdvancedSettings
    {
        public bool Enabled { get; set; }
        public bool ArchiveOldSamples { get; set; }
        public string OriginalExcelLocation { get; set; }
        public string CsvExportLocation { get; set; }
    }
}
