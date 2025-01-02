using Floppy.Application.DTOs.Response;
using Floppy.Application.Interfaces;
using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Sentry;

namespace Floppy.Application.Services
{
    public class PlaceEnquiryService : IPlaceEnquiryService
    {
        private readonly ICartRepository _cart;
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IEmailService _emailService;
        private readonly IPushNotificastionService _pushNotificastionService;    
        public PlaceEnquiryService(ICartRepository cart,IUserRepository userRepository, 
            ICartRepository cartRepository, ICategoryRepository categoryRepository, 
            IEmailService emailService,IPushNotificastionService pushNotificastionService)
        {
            _cart = cart;
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _categoryRepository = categoryRepository;
            _emailService=emailService;
            _pushNotificastionService = pushNotificastionService;
        }

        #region PlaceEnquiryMethod

        public async Task<ApiResponse<bool>> PlaceEnquiryForItem(PlaceEnquiryRequest request)
        {
            if (request.ItemIds == null || !request.ItemIds.Any())
            {
                return new ApiResponse<bool>
                {
                    Data = false,
                    Message = "Invalid or empty item IDs.",
                    Success = false
                };
            }

            // Get vendor ID if all items belong to the same vendor
            int? vendorId = _cart.GetVendorIdIfAllItemsSameVendor(request.ItemIds.Select(id => (decimal?)id).ToList());
            if (!vendorId.HasValue)
            {
                return new ApiResponse<bool>
                {
                    Data = false,
                    Message = "Items do not have the same vendor.",
                    Success = false
                };
            }

            // Retrieve user, item, and vendor details
            var userDetails = _userRepository.GetUserDetailsById(request.UserId).Result;
            var itemDetails = _categoryRepository.GetItemByIdAsync(request.ItemIds.FirstOrDefault()).GetAwaiter().GetResult();
            var vendorContactDetails = _cart.GetVendorContactDetailsById(vendorId.Value);

            if (userDetails == null || itemDetails == null || vendorContactDetails == null)
            {
                return new ApiResponse<bool>
                {
                    Data = false,
                    Message = "Failed to retrieve necessary details.",
                    Success = false
                };
            }
            var nameParts = (userDetails.Name ?? string.Empty).Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var firstName = nameParts.Length > 0 ? nameParts[0] : string.Empty;
            var lastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;
            var currentMaxLeadId = await _cartRepository.GetMaxLeadEntryIdAsync();
            decimal newLeadEntryId = Math.Max(currentMaxLeadId, 2000) + 1;
            bool isItemAddedForEnquiry = _cartRepository.AddCart(
                newLeadEntryId,
                request.UserId,
                itemDetails.Price,
                1,
                firstName,
                lastName,
                userDetails.Address,
                userDetails.EmailId,
                userDetails.MobileNo,
                userDetails.State,
                userDetails.City,
                userDetails.Pincode,
                DateTime.Now,
                "India",
                1,
                request.ItemIds.ToList(),
                null,
                null,
                request.Latitude,
                request.Longitude
            );

            if (!isItemAddedForEnquiry)
            {
                return new ApiResponse<bool>
                {
                    Data = false,
                    Message = "Failed to add enquiry to cart.",
                    Success = false
                };
            }
            if (vendorContactDetails != null)
            {
                var deviceToken = vendorContactDetails.GetType().GetProperty("DeviceToken")?.GetValue(vendorContactDetails, null)?.ToString();
                if (!string.IsNullOrEmpty(deviceToken))
                {
                    _pushNotificastionService.SendPushNotification(deviceToken, "placeEnquiry", request.UserId, Convert.ToInt32(newLeadEntryId),firstName,null);
                }
                if (!string.IsNullOrEmpty(userDetails.EmailId))
                {
                    _emailService.SendEmailAsync(userDetails.EmailId, null, "PlaceEnquiry",0);
                }

                return new ApiResponse<bool>
                {
                    Data = true,
                    Message = "Notification sent successfully.",
                    Success = true
                };
            }
            else
            {
                return new ApiResponse<bool>
                {
                    Data = false,
                    Message = "Failed to retrieve vendor contact details.",
                    Success = false
                };
            }
        }
        #endregion
    }
}
