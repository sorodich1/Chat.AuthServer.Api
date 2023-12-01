using System.ComponentModel.DataAnnotations;

namespace WebChatAPI.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "{0} должно быть не менее {1} и не более {2} символов", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "{0} должно быть не менее {1} и не более {2} символов", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "{0} должно быть не менее {1} и не более {2} символов", MinimumLength = 2)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessage = "Пароль и пароль подтверждения не совпадают.")]
        public string? ConfirmPassword { get; set; }
    }
}
