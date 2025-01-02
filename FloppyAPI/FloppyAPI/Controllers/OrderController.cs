using Floppy.Application.Interfaces;
using Floppy.Application.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FloppyAPI.Controllers
{
	[EnableCors]
	[Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ICartService _cartService;
        public OrderController(ICartService cartService)
        {
            _cartService = cartService;
        }
		#region FetchOrdersList
		[Authorize]
		[HttpGet("orderlist/{userId}")]
        public async Task<IActionResult> FetchOrderListByUserId(int userId)
        {
            var response = await _cartService.FetchorderDetails(userId);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
		#endregion

		#region ReviewOrderDetails
		[Authorize]
		[HttpGet("review_order_details/{orderItemId}")]
        public async Task<IActionResult> ReviewOrderDetailsById(int orderItemId)
        {
            var response = await _cartService.ReviewOrderDetails(orderItemId);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
		#endregion

		#region CancelOrder
		[Authorize]
		[HttpPost("CancelOrderById/{id}")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var response = await _cartService.CancelOrderAsync(id);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }
        }
		#endregion

		#region UpdateOrderDetails
		[Authorize]
		[HttpPost("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder([FromBody] LeadEntryUpdateModel request)
        {
            var response = await _cartService.UpdateOrderAsync(request.OrderId, request.NewSlot, request.NewDateTime, request.UserId);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }
        }
        #endregion
    }
}
