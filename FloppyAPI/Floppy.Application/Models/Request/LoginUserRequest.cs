using System.ComponentModel.DataAnnotations;

namespace Floppy.Application.Models.Request
{
    public class LoginUserRequest
    {
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [StringLength(14, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 14 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[\W_]).*$", ErrorMessage = "Password must contain at least one uppercase letter and one special character.")]
        public string? Password { get; set; }
        public string? MobileNumber { get; set; }


    }
}
