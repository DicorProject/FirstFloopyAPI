using Floppy.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Floppy.Domain.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Floppy.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailRepository _emailRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<EmailService> _logger; 
        public EmailService(IConfiguration configuration, IEmailRepository emailRepository, ICartRepository cartRepository,ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _emailRepository = emailRepository;
            _cartRepository = cartRepository;
            _logger = logger;   
        }

        #region SendEmail
        //public async Task SendEmailAsync(string toEmail, string firstName, string emailType)
        //{
        //    try
        //    {
        //        string body = string.Empty;
        //        string subject = string.Empty;
        //        int smtpId = 0;
        //        if (emailType == "Registration")
        //        {
        //            var emailTemplateData = _emailRepository.GetTemplateMasterAsyncByType("Customer Registration");
        //            smtpId = emailTemplateData?.Smtpid ?? 0;
        //            body = emailTemplateData?.Message ?? string.Empty;
        //            subject = emailTemplateData?.Subject ?? "Registration Email";

        //            if (!string.IsNullOrEmpty(body) && body.Contains("Dear"))
        //            {
        //                body = body.Replace("Dear", $"Dear {firstName}");
        //            }
        //        }
        //        else if (emailType == "PlaceEnquiry")
        //        {
        //            var emailTemplateData = _emailRepository.GetTemplateMasterAsyncByType("PlaceEnquiry");
        //            smtpId = emailTemplateData?.Smtpid ?? 1;
        //            body = emailTemplateData?.Message ?? "Thank you for your enquiry! We will get back to you shortly";
        //            subject = emailTemplateData?.Subject ?? "Enquiry Email";
        //        }
        //        else if (emailType == "Order")
        //        {
        //            var emailTemplateData = _emailRepository.GetTemplateMasterAsyncByType("OrderConfirmation");
        //            smtpId = emailTemplateData?.Smtpid ?? 0;
        //            body = emailTemplateData?.Message ?? string.Empty;
        //            subject = emailTemplateData?.Subject ?? "Order Confirmation Email";
        //        }
        //        else if(emailType== "PasswordReset")
        //        {
        //            var emailTemplateData = _emailRepository.GetTemplateMasterAsyncByType("PasswordReset");
        //            smtpId = emailTemplateData?.Smtpid ?? 0;
        //            body = emailTemplateData?.Message ?? string.Empty;
        //            subject = emailTemplateData?.Subject ?? "Password Reset Email";
        //        }
        //        else if (emailType == "cashOnDelivery")
        //        {
        //            var emailTemplateData = _emailRepository.GetTemplateMasterAsyncByType("OrderConfirmation");
        //            smtpId = emailTemplateData?.Smtpid ?? 0;
        //            body = emailTemplateData?.Message ?? string.Empty;
        //            subject = emailTemplateData?.Subject ??string.Empty;
        //        }
        //        else
        //        {
        //            throw new ArgumentException("Invalid EmailType specified.");
        //        }
        //        var smtpData = _emailRepository.GetBySmtpId(smtpId);
        //        if (smtpData == null)
        //        {
        //            throw new InvalidOperationException("SMTP configuration not found.");
        //        }

        //        var smtpHost = smtpData?.Host;
        //        var smtpPort = int.Parse(smtpData?.PortNo ?? "0");
        //        var smtpUsername = smtpData?.Login;
        //        var smtpPassword = smtpData?.Password;
        //        var fromEmail = smtpData?.Login;

        //        if (string.IsNullOrEmpty(smtpHost) || smtpPort == 0 || string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword))
        //        {
        //            throw new InvalidOperationException("Invalid SMTP configuration.");
        //        }

        //        // Create email message
        //        var email = new MimeMessage();
        //        email.From.Add(MailboxAddress.Parse(fromEmail));
        //        email.To.Add(MailboxAddress.Parse(toEmail)); 
        //        email.Subject = subject; 

        //        var messageBody = new TextPart(MimeKit.Text.TextFormat.Html)
        //        {
        //            Text = body 
        //        };
        //        email.Body = messageBody;

        //        // Send email
        //        using (var smtp = new SmtpClient())
        //        {
        //            await smtp.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
        //            await smtp.AuthenticateAsync(smtpUsername, smtpPassword);
        //            await smtp.SendAsync(email);
        //            await smtp.DisconnectAsync(true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message.ToString());
        //    }
        //}
        #endregion

        #region SendEmail
        public async Task SendEmailAsync(string toEmail, string firstName, string emailType, int? OrderId)
        {
            try
            {
                string body = string.Empty;
                string subject = string.Empty;
                int smtpId = 0;
                // Get order details if the email type is "Order"

                var orderDetails = (emailType == "Order" || emailType == "cashOnDelivery" || emailType== "PlaceEnquiry") && OrderId.HasValue
                    ? await _cartRepository.GetOrderDetailsByOrderIdAsync(OrderId.Value)
                    : null;
                var emailTemplateData = emailType switch
                {
                    "Registration" => _emailRepository.GetTemplateMasterAsyncByType("Customer Registration"),
                    "PlaceEnquiry" => _emailRepository.GetTemplateMasterAsyncByType("PlaceEnquiry"),
                    "Order" => _emailRepository.GetTemplateMasterAsyncByType("Service Booking"),
                    "PasswordReset" => _emailRepository.GetTemplateMasterAsyncByType("PasswordReset"),
                    "cashOnDelivery" => _emailRepository.GetTemplateMasterAsyncByType("Service Booking"),
                    "BookingCancellation" => _emailRepository.GetTemplateMasterAsyncByType("Booking Cancellation"),
                    _ => throw new ArgumentException("Invalid EmailType specified.")
                };

                smtpId = emailTemplateData?.Smtpid ?? 0;
                body = emailTemplateData?.Message ?? string.Empty;
                subject = emailTemplateData?.Subject ?? $"{emailType} Email";

                if (!string.IsNullOrEmpty(body) && body.Contains("Dear"))
                {
                    body = body.Replace("Dear", $"Dear {firstName}");
                }
                // Add order details to the email body if emailType is "Order"
                if (emailType == "cashOnDelivery" || emailType == "Order" && orderDetails != null)
                {
                    var culture = new CultureInfo("en-IN");
                    body += "<br/><br/>Order Details:<br/>";
                    body += $"Order Id: {OrderId}<br/>";
					body += $"Product Name: {orderDetails.Cart.ItemName}<br/>";
                    body += $"Quantity: {orderDetails.Cart.Quantity}<br/>";
                    body += $"Price: {string.Format(culture, "{0:C}", orderDetails.Cart.Price)}<br/>";
                }
                if(emailType == "PlaceEnquiry" && orderDetails != null)
                {
					body += "<br/><br/>Enquiry Details:<br/>";
					body += $"Enquiry Id: {orderDetails.Cart.Quantity}<br/>";
					body += $"Product Name: {orderDetails.Cart.ItemName}<br/>";
				}

                var smtpData = _emailRepository.GetBySmtpId(smtpId);
                if (smtpData == null)
                {
                    throw new InvalidOperationException("SMTP configuration not found.");
                }

                var smtpHost = smtpData.Host;
                var smtpPort = int.Parse(smtpData.PortNo ?? "0");
                var smtpUsername = smtpData.Login;
                var smtpPassword = smtpData.Password;
                var fromEmail = smtpData.Login;

                if (string.IsNullOrEmpty(smtpHost) || smtpPort == 0 || string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword))
                {
                    throw new InvalidOperationException("Invalid SMTP configuration.");
                }

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(fromEmail));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;

                var messageBody = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = body
                };
                email.Body = messageBody;

                // Send email
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect(smtpHost, smtpPort, SecureSocketOptions.StartTls);
                    smtp.Authenticate(smtpUsername, smtpPassword);
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }

            }
            catch (Exception ex )
            {
                 _logger.LogError(ex, "Failed to send email.");
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
        #endregion
    }
}
