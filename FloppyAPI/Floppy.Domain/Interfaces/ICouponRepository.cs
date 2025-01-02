using Floppy.Domain.Entities;

namespace Floppy.Domain.Interfaces
{
    public interface ICouponRepository
    {
        Task<List<CouponMaster>> GetAllCouponsAsync();
        Task<DiscountResult> ApplyCoupononPrice(coupon request);
    }
}
