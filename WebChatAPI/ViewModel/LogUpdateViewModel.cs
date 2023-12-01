using Calabonga.EntityFrameworkCore.Entities.Base;

namespace WebChatAPI.ViewModel
{
    public class LogUpdateViewModel : ViewModelBase
    {
        public string? Logger { get; set; }

        public string? Level { get; set; }

        public string? Message { get; set; }
    }
}
