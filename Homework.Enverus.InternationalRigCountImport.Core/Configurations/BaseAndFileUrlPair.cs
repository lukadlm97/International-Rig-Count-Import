using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework.Enverus.InternationalRigCountImport.Core.Configurations
{
    public record BaseAndFileUrlPair(string BaseUrl, string FileUrl, string UserAgent);
}
