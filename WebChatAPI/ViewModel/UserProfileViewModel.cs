using System.Collections.Generic;
using System;

namespace WebChatAPI.ViewModel
{
    public class UserProfileViewModel
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PositionName { get; set; }
    }
}
