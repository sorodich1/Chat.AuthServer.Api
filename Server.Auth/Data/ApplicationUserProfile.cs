using System;
using System.Collections.Generic;
using AutoMapper;
using Server.Auth.Data.Entityes;
using Server.Auth.ViewModel;

namespace Server.Auth.Data
{
    [AutoMap(typeof(RegisterViewModel))]
    public class ApplicationUserProfile : AuditableConf.Auditable
    {
        public Guid ApplicationUserProfileId { get; set; }
        public virtual ApplicationUsers ApplicationUser { get; set; }
        public ICollection<MicroservicePermission> Permissions { get; set; }
    }
}
