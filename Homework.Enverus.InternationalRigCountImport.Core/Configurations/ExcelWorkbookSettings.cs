

namespace Homework.Enverus.InternationalRigCountImport.Core.Configurations
{
    public class ExcelWorkbookSettings
    {
        public string Worksheet { get; set; }
        public int StartRow { get; set; }
        public int EndRow { get; set; }
        public int StartColumn { get; set; }
        public int EndColumn { get; set; }
        public int RowsPerYear { get; set; }
        public int Years { get; set; }
    }
}
