//using Microsoft.Extensions.Configuration;

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;

//namespace DAPManSWebReports.Domain.IdentityService
//{
//    public class JwtHandler
//    {
//        private readonly IConfiguration _configuration;

//        private readonly UserManager<ApplicationUser> _userManager;

//        public JwtHandler(IConfiguration configuration, UserManager<ApplicationUser> userManager)
//        {
//            _configuration = configuration;
//            _userManager = userManager;
//        }

//        /// <summary>
//        /// Метод для генерации токена
//        /// </summary>
//        /// <param name="user"></param>
//        /// <returns></returns>
//        public async Task<JwtSecurityToken> GetTokenAsync(ApplicationUser user)
//        {
//            var jwtOptions = new JwtSecurityToken(
//            // издатель токена
//            issuer: _configuration["JwtSettings:Issuer"],
//            // потребитель токена
//            audience: _configuration["JwtSettings:Audience"],
//            claims: await GetClaimsAsync(user),
//            //время жизни токена
//            expires: DateTime.Now.AddMinutes(Convert.ToDouble(
//            _configuration["JwtSettings:ExpirationTimeInMinutes"])),
//            //ключ для шифрования
//            signingCredentials: GetSigningCredentials());
//            return jwtOptions;
//        }

//        /// <summary>
//        /// Метод для подписания токена
//        /// </summary>
//        /// <returns></returns>
//        private SigningCredentials GetSigningCredentials()
//        {
//            var key = Encoding.UTF8.GetBytes(
//            _configuration["JwtSettings:SecurityKey"]);
//            var secret = new SymmetricSecurityKey(key);
//            return new SigningCredentials(secret,
//            SecurityAlgorithms.HmacSha256);
//        }

//        private async Task<List<Claim>> GetClaimsAsync(ApplicationUser user)
//        {
//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.Name, user.Email)
//            };
//            foreach (var role in await _userManager.GetRolesAsync(user))
//            {
//                claims.Add(new Claim(ClaimTypes.Role, role));
//            }
//            return claims;
//        }
//    }
//}
