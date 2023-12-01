using System;
using System.ComponentModel.DataAnnotations;
using Server.Auth.Data.Entityes;

namespace Server.Auth.Data.AuditableConf
{
    public abstract class Auditable : IAudiotable
    {
        protected Auditable() { }

        [Key]
        public Guid Id { get; set; }
        public DateTime CreateAt { get; set; }
        public string CreateBy { get; set; }
        public DateTime UpdateAt { get; set; }
        public string UpdateBy { get; set; }
    }
}
