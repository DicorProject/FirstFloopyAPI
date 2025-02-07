using Floppy.Application.DTOs.Response;
using Floppy.Application.Interfaces;
using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Floppy.Application.Services
{
    public class CouponService: ICouponService
    {
        private readonly ICouponRepository _couponRepository;
        private readonly ILogger<CouponService> _logger;    
        public CouponService(ICouponRepository couponRepository,ILogger<CouponService> logger)
        {
            _couponRepository = couponRepository;
            _logger = logger;   
        }
        #region FetchActiveCoupon
        public async Task<ApiResponse<List<CouponMaster>>> FetchCouponDetails()
        {
            var response = new ApiResponse<List<CouponMaster>>();

            try
            {
                var couponEntries = await _couponRepository.GetAllCouponsAsync();

                if (couponEntries != null && couponEntries.Any())
                {
                    response.Success = true;
                    response.Message = "Coupon details fetched successfully.";
                    response.Data = couponEntries;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No coupon found ";
                    response.Data = new List<CouponMaster>();
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred while fetching coupon details: {ex.Message}";
                response.Data = null;
            }
            return response;
        }

        #endregion

        #region ApplyCoupon
        public async Task<ApiResponse<DiscountResult>> ApplyCoupon(coupon request)
        {
            var response = new ApiResponse<DiscountResult>();

            try
            {
                // Call the method to apply the coupon and get the result
                var discountResult = await _couponRepository.ApplyCoupononPrice(request);

                if (discountResult == null)
                {
                    response.Success = false;
                    response.Message = "Invalid or inactive coupon.";
                    response.Data = new DiscountResult
                    {
                        DiscountedPrice = request.TotalPrice,
                        DiscountAmount = 0
                    };
                }
                else if (discountResult.DiscountedPrice == 0 && discountResult.DiscountAmount == 0)
                {
                    response.Success = false;
                    response.Message = "You have already used this coupon.";
                    response.Data = new DiscountResult
                    {
                        DiscountedPrice = request.TotalPrice,
                        DiscountAmount = 0
                    };
                }
                else
                {
                    response.Success = true;
                    response.Message = "Coupon applied successfully.";
                    response.Data = discountResult;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred while applying the coupon: {ex.Message}";
                response.Data = new DiscountResult
                {
                    DiscountedPrice = request.TotalPrice,
                    DiscountAmount = 0
                };
            }

            return response;
        }

        #endregion
    }
}
