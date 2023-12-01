using System.Collections.Generic;

namespace Server.Auth.Helpers.AppData
{
    public static class AppData
    {
        public const string AdministratorRoleName = "Administrator";
        public const string ManagerRoleName = "Manager";

        public static IEnumerable<string> Roles
        {
            get
            {
                yield return AdministratorRoleName;
                yield return ManagerRoleName;
            }
        }
    }
}
