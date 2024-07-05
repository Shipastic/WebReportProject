using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DAPManSWebReports.Domain.IdentityService.TokenServise
{
    /// <summary>
    /// Абстрактный класс для генерации токена
    /// </summary>
    public abstract class TokenGenerator
    {
        protected abstract string Role { get; }
        /// <summary>
        /// Фабричный метод для генерации токена
        /// </summary>
        /// <param name="username"></param>
        /// <param name="secretKey"></param>
        /// <param name="issuer"></param>
        /// <param name="audience"></param>
        /// <returns></returns>
        public async Task<string> GenerateTokenAsync(string username, string secretKey, string issuer, string audience)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, Role)
        };
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    /// <summary>
    /// Конкретный класс для генерации токена для роли USER
    /// </summary>
    public class UserTokenGenerator : TokenGenerator
    {
        protected override string Role => "user";
    }
    /// <summary>
    ///  Конкретный класс для генерации токена для роли ADMIN
    /// </summary>
    public class AdminTokenGenerator : TokenGenerator
    {
        protected override string Role => "admin";
    }
}
