﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework.Enverus.InternationalRigCountImport.Core.Repositories
{
    public interface ICsvRepository
    {
        Task<bool> SaveFile(string content, string fileName,CancellationToken cancellationToken = default);
    }
}
