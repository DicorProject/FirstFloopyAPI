using Floppy.Application.Interfaces;
using Floppy.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FloppyAPI.Controllers
{
	[EnableCors]
	[Route("api/[controller]")]
    [ApiController]
    public class PlaceEnquiryController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PlaceEnquiryController(IPaymentService paymentService)
        {
            _paymentService = paymentService; 
        }
        #region PlaceEnquiry
        //[Authorize]
        [HttpPost("placeenquiry")]
        public IActionResult Createplaceenquiry(PlaceEnquiryRequest request)
        {
            var order =_paymentService.PlaceEnquiryForItem(request);
            if (order == null)
            {
                return StatusCode(500, "Error creating order.");
            }

            return Ok(order);
        }
        #endregion

    }
}
