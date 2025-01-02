using Floppy.Application.DTOs.Response;
using Floppy.Application.Interfaces;
using Floppy.Application.Models.Request;
using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Floppy.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<CartService> _logger;
        private readonly ISMSService _smssService;
        public CartService(ICartRepository cartRepository, IEmailService emailService, ILogger<CartService> logger,ISMSService sMSService)
        {
            _cartRepository = cartRepository;
            _emailService = emailService;
            _logger = logger;   
            _smssService = sMSService;  
        }

        #region SaveOrderDetails
        public async Task<ApiResponse<string>> SaveOrderDetails(OrderRequest request)
        {
            var response = new ApiResponse<string>();

            try
            {
                var currentMaxLeadId = await _cartRepository.GetMaxLeadEntryIdAsync();
                int startingPoint = 1000;
                decimal newLeadEntryId = Math.Max(currentMaxLeadId, startingPoint) + 1;

                var isInserted = _cartRepository.AddCart(newLeadEntryId, request.UserId,
                                 request.TotalAmount, request.TotalQuantity, request.FirstName,
                                 request.LastName, request.Address, request.Email, request.Phone,
                                 request.State, request.City, request.ZipCode, request.Date,
                                 request.Country, 1, request.Products.Select(p => p.ProductId).ToList(),
                                 request.Slot, request.Coupon, request.Latitude, request.Longitude);

                if (isInserted)
                {
                    response.Success = true;
                    response.Message = "Order placed successfully.";
                    response.Data = newLeadEntryId.ToString();
                }
                else
                {
                    response.Success = false;
                    response.Message = "Failed to place the order.";
                }
            }
            catch (Exception ex)
            {
				_logger.LogError(ex, "An error occurred while placing the order");
				response.Success = false;
                response.Message = $"An error occurred while placing the order: {ex.Message}";
            }
            return response;
        }
        #endregion

        #region FetchorderListByUserId
        public async Task<ApiResponse<List<LeadWithCartItemsDTO>>> FetchorderDetails(int userId)
        {
            var response = new ApiResponse<List<LeadWithCartItemsDTO>>();

            try
            {
                var leadEntries = await _cartRepository.GetLeadsByUserIdAsync(userId);

                if (leadEntries != null && leadEntries.Any())
                {
                    response.Success = true;
                    response.Message = "Order details fetched successfully.";
                    response.Data = leadEntries;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No order details found for the given user.";
                    response.Data = new List<LeadWithCartItemsDTO>();
                }
            }
            catch (Exception ex)
            {
				_logger.LogError(ex, "An error occurred while retrieving addresses");
				response.Success = false;
                response.Message = $"An error occurred while fetching order details: {ex.Message}";
                response.Data = null;
            }
            return response;
        }
        #endregion

        #region ReviewOrderDetailsByOrderId
        public async Task<ApiResponse<LeadByProductModel>> ReviewOrderDetails(int orderId)
        {
            var response = new ApiResponse<LeadByProductModel>();

            try
            {
                // Fetch order details
                var orderDetails = await _cartRepository.GetLeadByProductIdAsync(orderId);

                if (orderDetails != null)
                {
                    response.Success = true;
                    response.Message = "Order details retrieved successfully.";
                    response.Data = orderDetails;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No order details found for the given order ID.";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
				_logger.LogError(ex, "An error occurred while retrieving the order details");
				response.Success = false;
                response.Message = $"An error occurred while retrieving the order details: {ex.Message}";
                response.Data = null;
            }

            return response;
        }
        #endregion

        #region AddItemInCart
        public async Task<ApiResponse<bool>> AddCartDetails(List<CartItemDTO> cartMasters)
        {
            var response = new ApiResponse<bool>();

            try
            {
                var hasDifferentVendorResults = await Task.WhenAll(cartMasters.Select(async cartItem =>
                    !await _cartRepository.CheckAndAddToCartAsync(cartItem.ItemId, cartItem.UserId)));
                if (hasDifferentVendorResults.Contains(true))
                {
                    // Find the first item that caused the vendor conflict
                    var conflictingItem = cartMasters
                        .Where((cartItem, index) => hasDifferentVendorResults[index]) // Use index to filter
                        .FirstOrDefault();

                    if (conflictingItem != null)
                    {
                        response.Success = false;
                        response.Message = $"Item with ID {conflictingItem.ItemId} cannot be added as it belongs to a different vendor.";
                        return response;
                    }
                }

                // If all items pass the check, proceed to add them
                var isSuccess = await _cartRepository.AddAsync(cartMasters);

                if (isSuccess)
                {
                    response.Success = true;
                    response.Message = "Items added successfully.";
                    response.Data = isSuccess;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No items were added.";
                }
            }
            catch (Exception ex)
            {
				_logger.LogError(ex, "An error occurred while adding items");
				response.Success = false;
                response.Message = $"An error occurred while adding items: {ex.Message}";
            }

            return response;
        }


        #endregion

        #region UpdateCartDetails
        public async Task<ApiResponse<bool>> UpdateCartDetails(List<CartItemDTO> cartMasters)
        {
            var response = new ApiResponse<bool>();

            try
            {
                // Call the repository method to perform the update operation
                var updateSuccessful = await _cartRepository.UpdateAsync(cartMasters);

                if (updateSuccessful)
                {
                    response.Success = true;
                    response.Message = "Items updated successfully.";
                    response.Data = true;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No items were updated.";
                    response.Data = false;
                }
            }
            catch (Exception ex)
            {
				_logger.LogError(ex, "An error occurred while updating items");
				response.Success = false;
                response.Message = $"An error occurred while updating items: {ex.Message}";
                response.Data = false;
            }

            return response;
        }
        #endregion

        #region GetCartListByUserId
        public async Task<ApiResponse<List<ResponseItemDTO>>> GetCartDetailsAsync(int userId)
        {
            var response = new ApiResponse<List<ResponseItemDTO>>();

            try
            {
                // Call the repository method to retrieve cart details
                var cartDetails = await _cartRepository.GetByIdAsync(userId);

                if (cartDetails.Any())
                {
                    response.Success = true;
                    response.Message = "Cart details retrieved successfully.";
                    response.Data = cartDetails;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No cart details found for the given user ID.";
                    response.Data = new List<ResponseItemDTO>();
                }
            }
            catch (Exception ex)
            {
				_logger.LogError(ex, "An error occurred while retrieving cart details");
				response.Success = false;
                response.Message = $"An error occurred while retrieving cart details: {ex.Message}";
                response.Data = new List<ResponseItemDTO>();
            }

            return response;
        }
        #endregion

        #region DeleteCartItem
        public async Task<ApiResponse<bool>> DeleteCartAsync(List<int> ids)
        {
            var response = new ApiResponse<bool>();

            try
            {
                var isDeleted = await _cartRepository.DeleteAsync(ids);

                if (isDeleted)
                {
                    response.Success = true;
                    response.Message = "Cart Item deleted successfully.";
                    response.Data = true;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No cart Item was found with the specified ID.";
                    response.Data = false;
                }
            }
            catch (Exception ex)
            {
				_logger.LogError(ex, "An error occurred while deleting the cart Item");
				response.Success = false;
                response.Message = $"An error occurred while deleting the cart Item: {ex.Message}";
                response.Data = false;
            }

            return response;
        }
        #endregion

        #region CancelOrderAsync
        public async Task<ApiResponse<bool>> CancelOrderAsync(int id)
        {
            var response = new ApiResponse<bool>();

            try
            {
                string UserEmail = _cartRepository.GetEmailFromLeadEntrymasterByLeadId(id);
                var isDeleted = await _cartRepository.CancelLeadEntryAsync(id);

                if (isDeleted)
                {
                    if (UserEmail != null)
                    {
                        _emailService.SendEmailAsync(UserEmail, null, "BookingCancellation", id);
                    }
                    response.Success = true;
                    response.Message = "Order canceled successfully.";
                    response.Data = true;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No order was found with the specified ID.";
                    response.Data = false;
                }
            }
            catch (Exception ex)
            {
				_logger.LogError(ex, "An error occurred while deleting the order");
				response.Success = false;
                response.Message = $"An error occurred while deleting the order: {ex.Message}";
                response.Data = false;
            }

            return response;
        }
        #endregion

        #region UpdateOrderAsync
        public async Task<ApiResponse<bool>> UpdateOrderAsync(int id, string newSlot, DateTime newDateTimestamp, int UserId)
        {
            var response = new ApiResponse<bool>();

            try
            {
                // Call the repository method to update the lead entry
                var isUpdated = await _cartRepository.UpdateLeadEntryAsync(id, newSlot, newDateTimestamp);

                if (isUpdated)
                {
                    //await _smssService.SendSMSForOrderReschedule("Reschedule", UserId);
                    response.Success = true;
                    response.Message = "Order updated successfully.";
                    response.Data = true;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No order was found with the specified ID.";
                    response.Data = false;
                }
            }
            catch (Exception ex)
            {
				_logger.LogError(ex, "An error occurred while updating the order");
				response.Success = false;
                response.Message = $"An error occurred while updating the order: {ex.Message}";
                response.Data = false;
            }

            return response;
        }
        #endregion

    }
}
