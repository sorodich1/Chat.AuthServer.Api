using Calabonga.EntityFrameworkCore.Entities.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebChatAPI.ViewModel
{
    public class LogCreateViewModel : IViewModel
    {
        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        [StringLength(255)]
        public string? Logger { get; set; }

        [Required]
        [StringLength(50)]
        public string? Level { get; set; }

        [Required]
        [StringLength(4000)]
        public string? Message { get; set; }

        public string? ThreadId { get; set; }

        public string? ExceptionMessage { get; set; }
    }
}
