using System.ComponentModel.DataAnnotations;
namespace Floppy.Application.Models.Request
{
	public class UserAddressRequest
	{
		[Required(ErrorMessage = "User ID is required.")]
		public int UserId { get; set; }

		[Required(ErrorMessage = "Address type is required.")]
		[StringLength(150, ErrorMessage = "Address type cannot exceed 100 characters.")]
		public string? AddressType { get; set; }

		[Required(ErrorMessage = "Location is required.")]
		[StringLength(200, ErrorMessage = "Location cannot exceed 200 characters.")]
		public string? Location { get; set; }

		[Required(ErrorMessage = "City is required.")]
		[StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
		public string? City { get; set; }

		[Required(ErrorMessage = "State is required.")]
		[StringLength(100, ErrorMessage = "State cannot exceed 100 characters.")]
		public string? State { get; set; }

		[StringLength(10, ErrorMessage = "State code cannot exceed 10 characters.")]
		public string? StateCode { get; set; }

		[Required(ErrorMessage = "Pin code is required.")]
		[StringLength(10, ErrorMessage = "Pin code cannot exceed 10 characters.")]
		[RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Pin code must be in the format XXXXX or XXXXX-XXXX.")]
		public string? PinCode { get; set; }

		[StringLength(150, ErrorMessage = "Area cannot exceed 100 characters.")]
		public string? Area { get; set; }

		[Required(ErrorMessage = "Country is required.")]
		[StringLength(100, ErrorMessage = "Country cannot exceed 100 characters.")]
		public string? Country { get; set; }

		[StringLength(10, ErrorMessage = "Country code cannot exceed 10 characters.")]
		public string? CountryCode { get; set; }
	}

}
