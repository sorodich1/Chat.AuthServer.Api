using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Server.SignalR.Infrastructure.Helpers
{
    public static class EnumHelpers<T> where T : struct
    {
        public static Dictionary<T, string> GetValuesWithDisplayNames()
        {
            var type = typeof(T);
            var r = type.GetEnumValues();
            var list = new Dictionary<T, string>();
            foreach (var element in r)
            {
                list.Add((T)element, GetDisplayValue((T)element));
            }
            return list;
        }

        public static T? Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static T? TryParse(string value)
        {
            if(Enum.TryParse(value, true, out T result))
            { 
                return result;
            }
            return null;
        }

        public static IEnumerable<string> GetNames()
        {
            return typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        public static IList<string> GetDisplayValues(Enum value)
        {
            return GetNames().Select(obj => GetDisplayValue(Parse(obj))).ToList();
        }

        public static string GetDisplayValue(T? value)
        {
            var filedInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = filedInfo.GetCustomAttributes
                (typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes?.Length > 0 && descriptionAttributes[0].ResourceType != null)
            {
                return LookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);
            }

            if(descriptionAttributes == null)
            {
                return string.Empty;
            }

            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }

        private static string LookupResource(Type resourceType, string name)
        {
            foreach(var staticProperty in resourceType.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if(staticProperty.PropertyType == typeof(ResourceManager))
                {
                    var resourceManager = (ResourceManager)staticProperty.GetValue(null, null);
                    return resourceManager.GetString(name);
                }
            }
            return name;
        }
    }
}
