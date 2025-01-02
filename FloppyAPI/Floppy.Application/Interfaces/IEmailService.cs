namespace Floppy.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string firstName, string emailType, int? OrderId);
    }
}
