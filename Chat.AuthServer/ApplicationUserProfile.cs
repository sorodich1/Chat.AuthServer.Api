using Calabonga.EntityFrameworkCore.Entities.Base;
using System.Collections.Generic;

namespace Chat.AuthServer
{
    public class ApplicationUserProfile : Auditable
    {
        /// <summary>
        /// Аккаунт
        /// </summary>
        public virtual ApplicationUser ApplicationUser { get; set; }
        /// <summary>
        /// Разрешение  для политике авторизации
        /// </summary>
        public ICollection<MicroservicePermission> Permission {  get; set; }
    }
}
