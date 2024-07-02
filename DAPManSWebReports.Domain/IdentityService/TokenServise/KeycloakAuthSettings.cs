using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Domain.IdentityService.TokenServise
{
    public static class KeycloakAuthSettings
    {
        public static void AddKeycloakAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtKeyCloakSettings = configuration.GetSection("KeyCloakSettings");
            var keycloakRealm       = jwtKeyCloakSettings["keycloakRealm"];
            var keycloakUrl         = jwtKeyCloakSettings["keycloakBaseUrl"] + keycloakRealm;
            var keycloakAudience    = jwtKeyCloakSettings["Audience"];

            services.AddAuthentication()
                .AddJwtBearer("KeycloakScheme", options =>
                {
                    options.Authority = keycloakUrl;
                    options.Audience = keycloakAudience;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = keycloakUrl,

                        ValidateAudience = true,
                        ValidAudience = keycloakAudience,

                        ValidateLifetime = true,

                        ValidateIssuerSigningKey = true
                    };
                });
        }
    }
}
