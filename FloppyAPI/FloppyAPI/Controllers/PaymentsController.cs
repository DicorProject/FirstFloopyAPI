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
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _payment;
        public PaymentsController(IPaymentService payment)
        {
            _payment = payment;
        }
        #region CreateOrder
        [Authorize]
        [HttpPost("create-order")]
        public IActionResult CreateOrder([FromBody] OrderRequest request)
        {
            var order = _payment.CreateOrder(request);

            if (order == null)
            {
                return StatusCode(500, "Error creating order.");
            }

            return Ok(order);
        }
        #endregion

        #region UpdatePaymentStatus
        [Authorize]
        [HttpPost("UpdatePaymentStatus")]
        public async Task<IActionResult> GetOrderDetails([FromBody] paymentUpdate request)
        {
            try
            {
                var orderDetails = _payment.UpdatepaymentDetailsByUserId(request);
                if (orderDetails == null)
                    return NotFound("Order not found.");

                return Ok(orderDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        #endregion

    }
}
