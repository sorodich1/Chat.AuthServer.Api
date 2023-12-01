using Calabonga.EntityFrameworkCore.Entities.Base;
using System;

namespace Chat.AuthServer
{
    public class MicroservicePermission : Auditable
    {
        /// <summary>
        /// Идентификатор профиля пользователя
        /// </summary>
        public Guid? ApplicationUserProfileId { get; set; }
        /// <summary>
        /// Имя политики авторизации
        /// </summary>
        public string? PolicyName { get; set; }
        /// <summary>
        /// Профиль пользователя
        /// </summary>
        public virtual ApplicationUserProfile? ApplicationUserProfile { get; set; }
        /// <summary>
        /// Описание текущего разрешения
        /// </summary>
        public string? Description { get; set; }
    }
}
