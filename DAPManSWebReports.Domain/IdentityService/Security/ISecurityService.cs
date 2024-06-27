namespace DAPManSWebReports.Domain.IdentityService.Security
{
    public interface ISecurityService
    {
        bool IsCurrentUserAdmin { get; }
        string[] CurrentUserPermissions { get; }
    }
}