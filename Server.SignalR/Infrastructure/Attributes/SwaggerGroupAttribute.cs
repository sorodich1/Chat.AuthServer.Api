using System;

namespace Server.SignalR.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SwaggerGroupAttribute : Attribute
    {
        public SwaggerGroupAttribute(string groupName)
        {
            GroupName = groupName;
        }
        /// <summary>
        /// Имя группы
        /// </summary>
        public string GroupName { get; set; }
    }
}
