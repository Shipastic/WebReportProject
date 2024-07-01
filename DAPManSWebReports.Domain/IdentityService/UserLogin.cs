using System.ComponentModel.DataAnnotations;

namespace DAPManSWebReports.Domain.IdentityService
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = null!;
    }
}
