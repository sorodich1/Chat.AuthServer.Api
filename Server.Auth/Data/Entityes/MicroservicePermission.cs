using System;

namespace Server.Auth.Data.Entityes
{
    public class MicroservicePermission : AuditableConf.Auditable
    {
        public Guid ApplicationUserProfileId { get; set; }
        public virtual ApplicationUserProfile ApplicationUserProfile { get; set; }
        public string PolicyName { get; set; }
        public string Description { get; set; }
    }
}
