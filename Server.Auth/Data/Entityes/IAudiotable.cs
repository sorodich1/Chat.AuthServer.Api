using System;

namespace Server.Auth.Data.Entityes
{
    public interface IAudiotable
    {
        Guid Id { get; set; }
        DateTime CreateAt { get; set; }
        string CreateBy { get; set; }
        DateTime UpdateAt { get; set; }
        string UpdateBy { get; set; }
    }
}
