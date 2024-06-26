using Microsoft.AspNetCore.Http;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DAPManSWebReports.Domain.Entities
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
