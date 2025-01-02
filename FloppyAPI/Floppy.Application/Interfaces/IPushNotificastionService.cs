namespace Floppy.Application.Interfaces
{
    public interface IPushNotificastionService
    {
        Task<bool> SendPushNotification(string deviceToken, string notificationType, int UserId, int OrderId,string UserName, string OrderAmount);
    }
}
