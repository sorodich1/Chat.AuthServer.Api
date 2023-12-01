using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Chat.AuthServer
{
    /// <summary>
    /// Пользователь по умолчанию для приложения.
    /// Добавляем данные профиля для пользователей приложения, добавляя свойства в класс ApplicationUser
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        //[Key]
        //public Guid? Id {  get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public string? FirstName { get; set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        public string? LastName { get; set; }
        /// <summary>
        /// Профиль авторизации
        /// </summary>
        public Guid? ApplicationUserId { get; set; }
        /// <summary>
        /// Подключённый пользователь
        /// </summary>
        public bool Enabled { get; set; } = false;
        /// <summary>
        /// Профиль
        /// </summary>
        public virtual ApplicationUserProfile ApplicationUserProfile { get; set; }
    }
}
