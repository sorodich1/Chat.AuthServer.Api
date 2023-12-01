using System;

namespace Server.SignalR.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class SwaggerFormAttribute : Attribute
    {
        public SwaggerFormAttribute(string parametrName, string description, bool hasFileUpload = true)
        {
            ParametrName = parametrName;
            Description = description;
            HasFileUpload = hasFileUpload;
        }

        /// <summary>
        /// Включение получения файлов
        /// </summary>
        public bool HasFileUpload { get; set; }

        /// <summary>
        /// Имя параметра
        /// </summary>
        public string ParametrName { get; set; }

        /// <summary>
        /// Краткое описание
        /// </summary>
        public string Description { get; set; }
    }
}
