using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DAPManSWebReports.Domain.ErrorReportService
{
    public class ReportError
    {
        [Required]
        public string? description { get; set; }
        [Required]
        public string? email { get; set; }
        [Required]
        public IFormFile? file { get; set; }
        [Required]
        public string? url { get; set; }
    }
}
