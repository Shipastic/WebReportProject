using DAPManSWebReports.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendErrorReportAsync(ReportError reportError);
    }
}
