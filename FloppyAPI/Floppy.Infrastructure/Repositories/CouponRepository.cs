using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Floppy.Infrastructure.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CouponRepository> _logger;
        public CouponRepository(ApplicationDbContext context,ILogger<CouponRepository> logger)
        {
            _context = context;
            _logger = logger;   
        }
		#region GetCouponList
		public async Task<List<CouponMaster>> GetAllCouponsAsync()
        {
			var coupons = await _context.CouponMaster
	                        .Where(c => c.ActivationStatus == 1 && c.ShowonDashboard==1)
	                        .AsNoTracking()
	                        .ToListAsync();


			return coupons;
        }
		#endregion

		#region ApplyCoupononPrice
		public async Task<DiscountResult> ApplyCoupononPrice(coupon request)
        {
            try
            {
                bool couponAlreadyUsed = await _context.CouponTrans
                    .AnyAsync(t => t.UserId == request.UserId && t.CouponId == request.CouponId);

                if (couponAlreadyUsed)
                {
                    return new DiscountResult { DiscountedPrice = 0, DiscountAmount = 0 };
                }

                var couponDetails = await _context.CouponMaster
                    .Where(x => x.CouponId == request.CouponId && x.Status == 1)
                    .FirstOrDefaultAsync();

                if (couponDetails == null)
                {
                    throw new ArgumentException("Invalid or inactive coupon.");
                }

                decimal totalPrice = (decimal)request.TotalPrice;
                decimal percentage = Convert.ToInt32(couponDetails.Percentage);
                decimal amount = couponDetails.Amount ?? 0;
                decimal discountAmount = 0;

                if (percentage > 0)
                {
                    discountAmount = (totalPrice * percentage) / 100;
                }
                else if (amount > 0)
                {
                    discountAmount = amount;
                }

                decimal discountedPrice = totalPrice - discountAmount;
                discountedPrice = Math.Max(discountedPrice, 0);

                var sql = "INSERT INTO CouponTrans (UserId, CouponId, CouponUsedDate,CouponCode) VALUES (@UserId, @CouponId, @TransactionDate,@CouponCode)";
                var parameters = new[]
                {
                new SqlParameter("@UserId", request.UserId),
                new SqlParameter("@CouponId", request.CouponId),
                new SqlParameter("@TransactionDate", DateTime.UtcNow),
                new SqlParameter("@CouponCode",request.CouponCode)
            };

                await _context.Database.ExecuteSqlRawAsync(sql, parameters);

                return new DiscountResult
                {
                    DiscountedPrice = discountedPrice,
                    DiscountAmount = discountAmount
                };
            }
            catch(Exception ex) 
            {
                return new DiscountResult();

			}

        }
		#endregion

	}
}
