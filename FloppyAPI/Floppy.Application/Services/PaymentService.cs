using cashfree_pg.Client;
using cashfree_pg.Model;
using Floppy.Application.Interfaces;
using Floppy.Application.Models.Request;
using Floppy.Application.Models.Response;
using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Floppy.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly string _xClientId;
        private readonly string _xClientSecret;
        private readonly string _xApiVersion;
        private readonly IConfiguration _configuration;
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<PaymentService> _logger;
        private readonly IEmailService _emailService;
        private readonly IPushNotificastionService _pushNotificastionService;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISMSService _smsService;
        public PaymentService(IConfiguration configuration, ICartRepository cartRepository, ILogger<PaymentService> logger, IEmailService emailService, IPushNotificastionService pushNotificastionService, IUserRepository userRepository, ICategoryRepository categoryRepository,ISMSService sMSService)
        {
            _configuration = configuration;
            _xClientId = _configuration["Cashfree:AppId"];
            _xClientSecret = _configuration["Cashfree:SecretKey"];
            _xApiVersion = _configuration["Cashfree:Version"];
            _cartRepository = cartRepository;
            _logger = logger;

            Cashfree.XClientId = _xClientId;
            Cashfree.XClientSecret = _xClientSecret;
            Cashfree.XEnvironment = Cashfree.SANDBOX;
            _emailService = emailService;
            _pushNotificastionService = pushNotificastionService;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
            _smsService=sMSService;
        }

        #region CreateOrderUsingCashFreePaymentIntegration 
        public CashFreePaymentResponse CreateOrder(OrderRequest request)
        {
            try
            {
                if (request.Products == null || request.Products.Count == 0)
                {
                    throw new ArgumentException("Product list cannot be empty.");
                }
                int firstVendorId = request.Products[0].VendorId;
                if (!request.Products.All(p => p.VendorId == firstVendorId))
                {
                    throw new InvalidOperationException("All products must belong to the same vendor.");
                }
                var currentMaxLeadId = _cartRepository.GetMaxLeadEntryIdAsync().GetAwaiter().GetResult();
                decimal newLeadEntryId = Math.Max(currentMaxLeadId, 2000) + 1;

                // Add cart details to the repository
                bool isCartAdded = _cartRepository.AddCart(
                    newLeadEntryId, request.UserId, request.TotalAmount, request.TotalQuantity,
                    request.FirstName, request.LastName, request.Address, request.Email,
                    request.Phone, request.State, request.City, request.ZipCode,
                    request.Date, request.Country, 1, request.Products.Select(p => p.ProductId).ToList(),
                    request.Slot, request.Coupon, request.Latitude, request.Longitude);
                if (isCartAdded)
                {
                    if (request.IsCashOnDelivery)
                    {
                        return new CashFreePaymentResponse
                        {
                            PaymentSessionId = null,
                            OrderId = newLeadEntryId.ToString(),
                            PaymentMode = "Cash on Delivery",
                            PaymentOrderReferenceId = null,
                        };
                    }
                    else
                    {
                        var createcashdata = CreateCashfreeOrder(new CreateOrderRequest(
                        null,
                        request.TotalAmount,
                        request.currency,
                        new CustomerDetails(request.UserId.ToString(), null, request.Phone),
                        null,
                        new OrderMeta(request.returnUrl, null, null),
                        null,
                        null,
                        null,
                        null
                    ));

                        if (createcashdata != null)
                        {
                            // Return successful response with payment session ID and order ID
                            return new CashFreePaymentResponse
                            {
                                PaymentSessionId = createcashdata.payment_session_id,
                                PaymentOrderReferenceId = createcashdata.order_id,
                                OrderId = newLeadEntryId.ToString(),
                            };
                        }
                        else
                        {
                            // Log and handle null response from Cashfree order creation
                            _logger.LogWarning("Cashfree order creation returned null.");
                        }
                    }

                }

                else
                {
                    _logger.LogWarning("Failed to add VendorId to the repository.");
                }
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, "Failed to create Cashfree order.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating the order.");
            }

            return null;
        }

        #endregion

        #region UpdatepaymentDetailsByUserId
        public DTOs.Response.ApiResponse<string> UpdatepaymentDetailsByUserId(paymentUpdate request)
        {
            var response = new DTOs.Response.ApiResponse<string>();

            try
            {
                string paymentMethod = null;
                string paymentStatus = null;
                string OrderId = null;
                string notificationType = null;
                // Check if Cash on Delivery (COD) is true
                if (request.IsCashOnDelivery)
                {
                    paymentMethod = "Cash on Delivery";
                    paymentStatus = "Pending";
                    OrderId = "COD_" + request.OrderId;
                    notificationType = "cashOnDelivery";
                }
                else
                {
                    // Get order details if it's not Cash on Delivery
                    var data = GetOrderDetails(request.PaymentReferenceOrderId).Result;
                    if (data == null)
                    {
                        response.Success = false;
                        response.Message = "Transaction canceled.";
                        response.Data = null;
                        return response;
                    }
                    paymentMethod = data?.PaymentGroup.ToString().ToUpper() ?? "UPI";
                    paymentStatus = data?.PaymentStatus ?? "Success";
                    OrderId = request.PaymentReferenceOrderId ?? null;
                    notificationType = "order";
                }
                bool isUpdate = _cartRepository.UpdatePaymentData(
                    request.UserId,
                    paymentMethod,
                    paymentStatus,
                    OrderId
                ).GetAwaiter().GetResult();

                if (isUpdate)
                {
                    // Retrieve the order confirmation ID
                    var orderConfirmId = _cartRepository.GetLeadEntryId(request.OrderId, request.UserId).GetAwaiter().GetResult();
                    int? vendorId = _cartRepository.GetLeadVendorTransDetails(orderConfirmId);
                    string OrderAmount = _cartRepository.GetTotalAmountFromLeadEntrymasterByLeadId(Convert.ToInt32(request.OrderId));
					SendEmailAndNotification(vendorId, request.UserId, request.Email, Convert.ToInt32(request.OrderId), notificationType, OrderAmount,null).GetAwaiter().GetResult();
                    response.Success = true;
                    response.Message = "Your order has been placed.";
                    response.Data = orderConfirmId.ToString();
                }
                else
                {
                    response.Success = false;
                    response.Message = "No records updated. Please check the UserId or order status.";
                    response.Data = "No updates performed";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating payment details.");
                response.Success = false;
                response.Message = $"An error occurred while updating payment details: {ex.Message}";
                response.Data = null;
            }

            return response;
        }

        #endregion

        #region PlaceEnquiryMethod
        public DTOs.Response.ApiResponse<bool> PlaceEnquiryForItem(PlaceEnquiryRequest request)
        {
            if (request.ItemIds == null || !request.ItemIds.Any())
            {
                return new DTOs.Response.ApiResponse<bool>
                {
                    Data = false,
                    Message = "Invalid or empty item IDs.",
                    Success = false
                };
            }

            // Get vendor ID if all items belong to the same vendor
            int? vendorId = _cartRepository.GetVendorIdIfAllItemsSameVendor(request.ItemIds.Select(id => (decimal?)id).ToList());
            if (!vendorId.HasValue)
            {
                return new DTOs.Response.ApiResponse<bool>
                {
                    Data = false,
                    Message = "Items do not have the same vendor.",
                    Success = false
                };
            }

            // Retrieve user, item, and vendor details
            var userDetails = _userRepository.GetUserDetailsById(request.UserId).Result;
            var itemDetails = _categoryRepository.GetItemByIdAsync(request.ItemIds.FirstOrDefault()).GetAwaiter().GetResult();
            var vendorContactDetails = _cartRepository.GetVendorContactDetailsById(vendorId.Value);

            if (userDetails == null || itemDetails == null || vendorContactDetails == null)
            {
                return new DTOs.Response.ApiResponse<bool>
                {
                    Data = false,
                    Message = "Failed to retrieve necessary details.",
                    Success = false
                };
            }
            var nameParts = (userDetails.Name ?? string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var firstName = nameParts.Length > 0 ? nameParts[0] : string.Empty;
            var lastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;
            var currentMaxLeadId = _cartRepository.GetMaxLeadEntryIdAsync().GetAwaiter().GetResult();
            decimal newLeadEntryId = Math.Max(currentMaxLeadId, 2000) + 1;
            bool isItemAddedForEnquiry = _cartRepository.AddCart(
                newLeadEntryId,
                request.UserId,
                itemDetails.Price,
                1,
                firstName,
                lastName,
                request.Address,
                userDetails.EmailId,
                userDetails.MobileNo,
                request.State,
                request.City,
                request.ZipCode,
                DateTime.Now,
                request.Country,
                1,
                request.ItemIds.ToList(),
                null,
                null,
                request.Latitude,
                request.Longitude
            );

            if (!isItemAddedForEnquiry)
            {
                return new DTOs.Response.ApiResponse<bool>
                {
                    Data = false,
                    Message = "Failed to add enquiry to cart.",
                    Success = false
                };
            }
            if (vendorContactDetails != null)
            {
                var deviceToken = vendorContactDetails.GetType().GetProperty("DeviceToken")?.GetValue(vendorContactDetails, null)?.ToString();
                SendEmailAndNotification(vendorId.Value, request.UserId, userDetails.EmailId, Convert.ToInt32(newLeadEntryId), "PlaceEnquiry",null,itemDetails.ItemName).GetAwaiter().GetResult();
                return new DTOs.Response.ApiResponse<bool>
                {
                    Data = true,
                    Message = "Successfully placed an inquiry for the item.",
                    Success = true
                };
            }
            else
            {
                return new DTOs.Response.ApiResponse<bool>
                {
                    Data = false,
                    Message = "Failed to retrieve vendor contact details.",
                    Success = false
                };
            }
        }
        #endregion

        #region SendEmailAndNotification
        private async Task SendEmailAndNotification(int? vendorId, int UserId, string Email, int OrderId, string notificationType, string OrderAmount,string serviceName)
        {
            try
            {
                var vendorContactDetails = _cartRepository.GetVendorContactDetailsById(vendorId.Value);
                if (vendorContactDetails != null)
                {
                    var deviceToken = vendorContactDetails.GetType().GetProperty("DeviceToken")?.GetValue(vendorContactDetails, null)?.ToString();

                    // Send push notification if device token exists
                    if (!string.IsNullOrEmpty(deviceToken))
                    {
                        await SendPushNotificationAsync(deviceToken, notificationType, UserId, OrderId,OrderAmount);
                    }
					if (notificationType == "cashOnDelivery" || notificationType == "order")
					{
						await _smsService.SendSMSForOrderConfirmation("Order Confirmation", UserId);
					}
					if (notificationType == "PlaceEnquiry")
					{
						await _smsService.SendSMSForEnquiry("Enquiry", UserId, serviceName);
					}
					// Send email if the email address is valid
					if (!string.IsNullOrEmpty(Email))
                    {
                        await SendEmailAsync(Email, notificationType, OrderId);
                    }

                    
                }
                else
                {
                    Console.WriteLine("Failed to retrieve vendor contact details.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        #endregion

        #region EmailSend
        private async Task SendEmailAsync(string email, string notificationType, int OrderId)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    await _emailService.SendEmailAsync(email, null, notificationType, OrderId);
                    Console.WriteLine("Email sent successfully.");
                }
                else
                {
                    Console.WriteLine("Email address is empty or null.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending the email: {ex.Message}");
            }
        }

        #endregion

        #region PushNotification

        // Separate method for sending a push notification
        private async Task SendPushNotificationAsync(string deviceToken, string notificationType, int userId, int orderId, string OrderAmount)
        {
            try
            {
                if (!string.IsNullOrEmpty(deviceToken))
                {
                    await _pushNotificastionService.SendPushNotification(deviceToken, notificationType, userId, orderId, null,OrderAmount);
                    Console.WriteLine("Push notification sent successfully.");
                }
                else
                {
                    Console.WriteLine("Device token is empty or null.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending the push notification: {ex.Message}");
            }
        }

        #endregion

        #region GetOrderDetails
        private async Task<OrderEntityDetails> GetOrderDetails(string orderId)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(System.Net.Http.HttpMethod.Get, $"https://sandbox.cashfree.com/pg/orders/{orderId}/payments");

            // Set the necessary headers
            request.Headers.Add("accept", "application/json");
            request.Headers.Add("x-api-version", _configuration["Cashfree:Version"]);
            request.Headers.Add("x-client-id", _configuration["Cashfree:AppId"]);
            request.Headers.Add("x-client-secret", _configuration["Cashfree:SecretKey"]);

            try
            {
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode(); 
                var jsonString = await response.Content.ReadAsStringAsync();
                var orderEntities = JsonConvert.DeserializeObject<List<OrderEntityDetails>>(jsonString);
                return orderEntities.FirstOrDefault();
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "Failed to fetch order details from Cashfree.");
                return null;
            }
        }

        #endregion

        #region CreateCashfreeOrder
        private cashfree_pg.Model.OrderEntity CreateCashfreeOrder(CreateOrderRequest createOrderRequest)
        {
            var cashfree = new Cashfree();
            var result = cashfree.PGCreateOrder(_xApiVersion, createOrderRequest, null, null, null);
            return result.Content as cashfree_pg.Model.OrderEntity;
        }
        #endregion
    }
}
