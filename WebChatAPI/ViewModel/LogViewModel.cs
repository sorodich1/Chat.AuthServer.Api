using Calabonga.EntityFrameworkCore.Entities.Base;
using System;

namespace WebChatAPI.ViewModel
{
    public class LogViewModel : ViewModelBase
    {
        public DateTime CreatedAt { get; set; }

        public string? Logger { get; set; }

        public string? Level { get; set; }

        public string? Message { get; set; }

        public string? ThreadId { get; set; }

        public string? ExceptionMessage { get; set; }
    }
}
