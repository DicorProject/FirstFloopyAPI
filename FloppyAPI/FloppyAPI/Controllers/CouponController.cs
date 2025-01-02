using Floppy.Application.Interfaces;
using Floppy.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FloppyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService; 
        }
        #region FetchCoupon
        [HttpGet("CouponList")]
        public async Task<IActionResult> GetItemList()
        {
            var response = await _couponService.FetchCouponDetails();
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region ApplyCoupon
        [HttpPost("AddCoupon")]
        public async Task<IActionResult> AddCoupon([FromBody] coupon request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _couponService.ApplyCoupon(request);   

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion
    }
}
