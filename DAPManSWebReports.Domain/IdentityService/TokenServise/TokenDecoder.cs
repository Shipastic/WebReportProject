using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Domain.IdentityService.TokenServise
{
    /// <summary>
    /// Класс для декодирования токена
    /// </summary>
    public static class TokenDecoder
    {
        /// <summary>
        /// Метод для получения роли из токена
        /// </summary>
        /// <param name="token"></param>
        /// <param name="secretKey"></param>
        /// <param name="issuer"></param>
        /// <param name="audience"></param>
        /// <returns></returns>
        public static string GetRoleFromToken(string token, string secretKey, string issuer, string audience)
        {
            var principal = TokenUtility.ValidateToken(token, secretKey, issuer, audience);


            var roleClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            return roleClaim?.Value;
        }
    }
}
