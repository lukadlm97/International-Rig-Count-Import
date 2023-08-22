
namespace Homework.Enverus.InternationalRigCountImport.Core.Exceptions
{
    public class MissingImporterOrExporterConfigurationsExceptions : Exception
    {
        public MissingImporterOrExporterConfigurationsExceptions()
        {
        }

        public MissingImporterOrExporterConfigurationsExceptions(string message)
            : base(message)
        {
        }

        public MissingImporterOrExporterConfigurationsExceptions(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
