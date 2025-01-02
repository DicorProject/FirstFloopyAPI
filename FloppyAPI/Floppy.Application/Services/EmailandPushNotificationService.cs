using Floppy.Application.Interfaces;

namespace Floppy.Application.Services
{
    public class EmailandPushNotificationService : IEmailandPushNotificationService
    {
        private readonly IEmailService _emailService;
        private readonly IPushNotificastionService _pushNotificastionService;
        public EmailandPushNotificationService(IEmailService emailService,IPushNotificastionService pushNotificastionService)
        {
            _emailService = emailService;
            _pushNotificastionService = pushNotificastionService;   
        }
        public void SendNotifications(string deviceToken, string userEmail, int UserId, int leadEntryId)
        {

            if (!string.IsNullOrEmpty(userEmail))
            {
                _emailService.SendEmailAsync(userEmail, null, "PlaceEnquiry",0);
            }

            if (!string.IsNullOrEmpty(deviceToken))
            {
                //_pushNotificastionService.SendPushNotification(deviceToken, "placeEnquiry", UserId, leadEntryId);
            }
        }
    }
}
