using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Server.Auth.ViewModel;
using System;
using System.Collections.Generic;

namespace Server.Auth.Data.Entityes
{
    [AutoMap(typeof(RegisterViewModel))]
    public class ApplicationUsers : IdentityUser<Guid>
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public Guid ApplicationUserId { get; set; }
        public bool Enabled { get; set; } = false;
        public ICollection<MicroservicePermission> Permissions { get; set; }
        public virtual ApplicationUserProfile ApplicationUserProfile { get; set; }
    }
}
