using System;

namespace Chat.AuthServer.Interfaces
{
    public interface IAuditable
    {
        DateTime CreatedAt { get; set; }
        string CreatedBy { get; set; }
        DateTime UpdateAt { get; set; }
        string UpdateBy { get; set; }
    }
}
