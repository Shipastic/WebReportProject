using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Domain.IdentityService.Security
{
    public class SecurityService : ISecurityService
    {
        public bool IsCurrentUserAdmin => throw new NotImplementedException();

        public string[] CurrentUserPermissions => throw new NotImplementedException();
    }
}
