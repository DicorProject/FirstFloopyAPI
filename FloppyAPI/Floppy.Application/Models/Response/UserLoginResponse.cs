namespace Floppy.Application.Models.Response
{
    public class UserLoginResponse
    {
        public int UserId { get; set; }
        public string? Token { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }   

    }
}
