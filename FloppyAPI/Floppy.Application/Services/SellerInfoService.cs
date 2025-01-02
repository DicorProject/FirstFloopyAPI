using Floppy.Application.DTOs.Response;
using Floppy.Application.Interfaces;
using Floppy.Application.Models.Response;
using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Floppy.Application.Services
{
    public class SellerInfoService : ISellerInfoService
    {
        private readonly ISellerInfoRepository _sellerInfoRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<SellerInfoService> _logger;    
		private readonly IRatingRepository _ratingRepository;
        public SellerInfoService(ISellerInfoRepository sellerInfoRepository,ILogger<SellerInfoService> logger,ICategoryRepository categoryRepository,IRatingRepository ratingRepository)
        {
            _sellerInfoRepository = sellerInfoRepository; 
            _categoryRepository = categoryRepository;
            _logger = logger;  
			_ratingRepository = ratingRepository;	
        }

        #region Fetch Vendor Registration Details ById 
        public async Task<ApiResponse<VendorRegistration>> GetVendorRegistrationDetailsData(int Id)
        {
            var response = new ApiResponse<VendorRegistration>();

            try
            {
                var vendorDetails = await _sellerInfoRepository.FetchVendorRegistrationDetails(Id);

                if (vendorDetails != null)
                {
                    response.Success = true;
                    response.Message = "Vendor registration details retrieved successfully.";
                    response.Data = vendorDetails;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Vendor registration details not found.";
                    response.Data = null;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                response.Data = null;
            }

            return response;
        }
		#endregion

		public async Task<ApiResponse<ItemWithVendorDetailsResponse>> GetItemListByVendorId(int vendorId, double latiude, double longitude, int ItemId)
		{
			var response = new ApiResponse<ItemWithVendorDetailsResponse>();

			try
			{
				var Items = await _categoryRepository.GetItemListWithDetailsByVendorIdAsync(vendorId);

				if (Items != null && Items.Any())
				{
					var vendorIds = Items.Select(item => item.vendorid).Distinct().ToList();
					var vendors = await _categoryRepository.GetNearbyVendorsByIdsAndLocation(vendorIds, latiude, longitude);
					var itemIds = Items.Select(item => item.id).Distinct().ToList();
					var reviews = await _ratingRepository.GetRatingsByItemIdsAsync(itemIds);
					var itemsWithVendorDetails = Items.Select(item => new ItemWithVendorDetails
						{		
							Item = item,
							Reviews = reviews.Where(r => r.ItemId == item.id).ToList(),
							Vendor = vendors.FirstOrDefault(v => v.Id == item.vendorid),
					     }).Where(itemWithVendorDetails => itemWithVendorDetails.Item.id != ItemId)
						.ToList();
					response.Success = true;
					response.Message = "Items retrieved successfully";
					response.Data = new ItemWithVendorDetailsResponse
					{
						Items = itemsWithVendorDetails,
					};
				}
				else
				{
					response.Success = false;
					response.Message = "No items found for the given vendor ID";
					response.Data = new ItemWithVendorDetailsResponse();
				}
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = $"An error occurred: {ex.Message}";
				response.Data = null;
			}

			return response;
		}
	}
}
