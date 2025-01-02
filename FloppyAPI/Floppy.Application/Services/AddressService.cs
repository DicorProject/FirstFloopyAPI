using Floppy.Application.DTOs.Response;
using Floppy.Application.Interfaces;
using Floppy.Application.Models.Request;
using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Floppy.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly ILogger<AddressService> _logger;
        public AddressService(IAddressRepository addressRepository, ILogger<AddressService> logger)
        {
            _addressRepository = addressRepository;
            _logger = logger;   
        }

        #region SaveAddress
        public async Task<ApiResponse<string>> SaveUserAddress(UserAddressRequest request)
        {
            var response = new ApiResponse<string>();

            try
            {
				await _addressRepository.SaveAddress(
			            request.UserId,
			            request.AddressType,
			            request.Location,
			            request.City,
			            request.State,
			            request.PinCode,
			            request.Area,
			            request.Country,
			            request.StateCode,
			            request.CountryCode);
				response.Success = true;
                response.Message = "Address saved successfully.";
                response.Data = string.Empty;
            }
            catch (Exception ex)
            {
				_logger.LogError(ex, "An error occurred while saving the address for UserId: {UserId}", request.UserId);
				response.Success = false;
                response.Message = $"An error occurred while saving the address: {ex.Message}";
            }

            return response;
        }
        #endregion

        #region GetAddressByUserId
        public async Task<ApiResponse<List<AddressMasterData>>> GetUserAddresDetailsByUserId(int userId)
        {
            var response = new ApiResponse<List<AddressMasterData>>()
            {
                Data = new List<AddressMasterData>()
            };

            try
            {
                var addresses = await _addressRepository.GetUserAddressByUserId(userId);

                if (addresses.Any())
                {
                    response.Success = true;
                    response.Message = "Addresses retrieved successfully.";
                    response.Data = addresses;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No addresses found for the specified user ID.";
                }
            }
            catch (Exception ex)
            {
				_logger.LogError(ex, "An error occurred while retrieving addresses");
				response.Success = false;
                response.Message = $"An error occurred while retrieving addresses: {ex.Message}";
            }

            return response;
        }
        #endregion
    }
}
