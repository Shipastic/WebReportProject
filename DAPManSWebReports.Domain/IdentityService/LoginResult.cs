using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPManSWebReports.Domain.IdentityService
{
    public class LoginResult
    {
        /// <summary>
        /// True - если успешный вход, False - в случае неудачи
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Сообщение о результате входа
        /// </summary>
        public string Message { get; set; } = null!;
        /// <summary>
        /// JWT токен если вход успешен, NULL - если нет
        /// </summary>
        public string? Token { get; set; }
    }
}
