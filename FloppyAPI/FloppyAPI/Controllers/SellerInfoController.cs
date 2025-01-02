using Floppy.Application.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FloppyAPI.Controllers
{
	[EnableCors]
	[Route("api/[controller]")]
    [ApiController]
    public class SellerInfoController : ControllerBase
    {
        private readonly ISellerInfoService _sellerInfoService;
        public SellerInfoController(ISellerInfoService sellerInfoService)
        {
            _sellerInfoService = sellerInfoService;
        }

        #region GetsellerInfoDetailsById
        [HttpGet("GetsellerInfoDetailsById/{id}")]
        public async Task<IActionResult> GetVendorRegistrationDetails(int id)
        {
            var response = await _sellerInfoService.GetVendorRegistrationDetailsData(id);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
		#endregion

		#region GetAllItemListsByVendorIdWise
		[HttpGet("GetAllItemListsByVendorId")]
		public async Task<IActionResult> GetAllItemListsByVendorId(int vendorId, double latiude, double longitude, int ItemId)
		{
			var response = await _sellerInfoService.GetItemListByVendorId(vendorId, latiude, longitude,ItemId);

			if (response.Success)
			{
				return Ok(response);
			}
			return BadRequest(response);
		}
		#endregion

	}
}
