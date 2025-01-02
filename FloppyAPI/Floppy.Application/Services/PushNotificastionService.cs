using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Floppy.Application.Interfaces;
using Floppy.Domain.Interfaces;
using Google;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Floppy.Application.Services
{
    public class PushNotificationService : IPushNotificastionService
    {
        private readonly FirebaseApp _firebaseApp;
        private readonly ICategoryService _categoryService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<PushNotificationService> _logger;
        public PushNotificationService(IConfiguration configuration, ICategoryService categoryService, IUserRepository userRepository, ILogger<PushNotificationService> logger)
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "firstfloppy-a9337-firebase-adminsdk-m828g-74352368ec.json");
                _firebaseApp = FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(filePath)
                });
            }
            else
            {
                _firebaseApp = FirebaseApp.DefaultInstance;
            }
            _categoryService = categoryService;
            _userRepository=userRepository;
            _logger = logger;   
        }

        #region SendNotificationAsync
        public async Task<bool> SendPushNotification(string deviceToken, string notificationType,int UserId , int OrderId,string Name,string OrderAmount)
        {
            try
            {
                string title = null;
                string sound = null;    
                string message = null;
                string userName = null;
                var UserDetails = _userRepository.GetUserDetailsById(UserId);
                string city = UserDetails?.Result?.City;
                string state = UserDetails?.Result?.State;

                string location = $"{city}, {state}".Trim().TrimEnd(',');
                if (Name== null)
                {
                    userName = UserDetails?.Result.Name ?? Name;
                }
                else
                {
                    userName = Name;
                }
                if (notificationType == "PlaceEnquiry")
                {
                    title = $"New Enquiry from {userName}";
                    //message = $"{userName} has made a new enquiry.Enquiry ID:{OrderId}.Location:{location}";
                    message = $"{userName} has placed a new Enquiry and The Enquiry Id is: {OrderId}";
                    sound = "new_lead.caf";

                }
                else if (notificationType == "order")
                {
                    title = "New Lead";
                    // Customize message if OrderId is provided
                    if (OrderId > 0)
                    {
                        message = $"{userName}, has placed a new order with {OrderAmount} and The order Id is {OrderId}.";
                    }
                    else
                    {
                        message = $"{userName}, has placed a new order.";  
                    }
                    sound = "new_lead.caf";
                }
                else if (notificationType == "cashOnDelivery")
                {
                    title = "New Lead";
                    message = $"{userName}, has placed a new order with {OrderAmount} and The order Id is {OrderId}, and payment will be handled via cash on delivery..";
                    sound = "default";
                }
                if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(message))
                {
                    throw new InvalidOperationException("Notification type is not supported.");
                }

                var notificationMessage = new Message()
                {
                    Token = deviceToken,
                    Notification = new Notification
                    {
                        Title = title,
                        Body = message
                    },
                    Android = new AndroidConfig
                    {
                        Priority = Priority.High, 
                        Notification = new AndroidNotification
                        {
                            Title = title,
                            Body = message,
                            Sound = "", 
                            ClickAction = "OPEN_ACTIVITY", 
                            Icon = "ic_notification" 
                        }
                    },
                    Apns = new ApnsConfig
                    {
                        Aps = new Aps
                        {
                            Alert = new ApsAlert
                            {
                                Title = title,
                                Body = message
                            },
                            Sound = sound
                        }
                    },
                    Data = new Dictionary<string, string>()
                    {
                        { "Title", title },
                        { "Message", message }
                    }
                };

                var response = await FirebaseMessaging.DefaultInstance.SendAsync(notificationMessage);
                Console.WriteLine($"Notification sent successfully: {response}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending notification: {ex.Message}");
                return false;
            }
        }

        #endregion
    }
}
