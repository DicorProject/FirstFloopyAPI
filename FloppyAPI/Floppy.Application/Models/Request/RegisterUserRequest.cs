using System.ComponentModel.DataAnnotations;

namespace Floppy.Application.DTOs.Request
{
    public class RegisterUserRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [Phone]
        public string? MobileNumber { get; set; }

        [Required]
        [StringLength(14, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 14 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[\W_]).*$", ErrorMessage = "Password must contain at least one uppercase letter and one special character.")]
        public string? Password { get; set; }


    }
}
