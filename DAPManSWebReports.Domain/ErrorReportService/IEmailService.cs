namespace DAPManSWebReports.Domain.ErrorReportService
{
    public interface IEmailService
    {
        Task SendErrorReportAsync(ReportError reportError);
    }
}
