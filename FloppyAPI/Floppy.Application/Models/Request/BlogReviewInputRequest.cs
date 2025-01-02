namespace Floppy.Application.Models.Request
{
	public class BlogReviewInputRequest
	{
		public int? BlogId { get; set; }
		public int? UserReview { get; set; }
		public int? UserId { get; set; }
		public string? Name { get; set; }
		public string? Email { get; set; }
		public string? Website { get; set; }
		public int IsSaveNameEmailandWebsite { get; set; }
		public string Comment { get; set; }
	}
}
