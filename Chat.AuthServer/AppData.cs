using System.Collections.Generic;

namespace Chat.AuthServer
{
    public static partial class AppData
    {
        public const string ServiceName = "IdentityModule";

        public const string SystemAdministratorRoleNames = "Administrator";

        public const string ManagerRoleName = "Manager";

        public static IEnumerable<string> Roles
        {
            get
            {
                yield return ManagerRoleName;
                yield return SystemAdministratorRoleNames;
            }
        }
    }
}
