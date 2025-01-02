namespace Floppy.Application.Interfaces
{
    public interface IEmailandPushNotificationService
    {
        void SendNotifications(string deviceToken, string userEmail, int UserId, int leadEntryId);
    }
}
