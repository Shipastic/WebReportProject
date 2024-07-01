using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DAPManSWebReports.Domain.IdentityService.TokenServise
{
    /// <summary>
    /// Сервис для создания токена используя фабрику
    /// </summary>
    public  class JwtHandler
    {
        public async Task<string> GetTokenAsync(string username,    
                                                string secretKey, 
                                                string issuer, 
                                                string audience,
                                                string role)
        {
            var tokenGenerator = TokenFactory.CreateTokenGenerator(role);

            return await tokenGenerator.GenerateTokenAsync(username, secretKey, issuer, audience);
        }
    }
}
