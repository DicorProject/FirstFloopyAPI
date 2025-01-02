using Floppy.Application.DTOs.Response;
using Floppy.Domain.Entities;

namespace Floppy.Application.Interfaces
{
    public interface ICouponService
    {
        Task<ApiResponse<List<CouponMaster>>> FetchCouponDetails();
        Task<ApiResponse<DiscountResult>> ApplyCoupon(coupon request);
    }
}
