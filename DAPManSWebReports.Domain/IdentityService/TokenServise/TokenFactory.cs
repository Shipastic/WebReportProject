using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Domain.IdentityService.TokenServise
{
   /// <summary>
   /// Фабрика для создания нужного типа объекта
   /// </summary>
    public static class TokenFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static TokenGenerator CreateTokenGenerator(string role)
        {
            return role switch
            {
                "admin" => new AdminTokenGenerator(),
                "user" => new UserTokenGenerator(),
                _ => throw new ArgumentException("Invalid role", nameof(role))
            };
        }
    }
}
