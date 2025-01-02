using Floppy.Application.Interfaces;
using Floppy.Application.Models.Request;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FloppyAPI.Controllers
{
	[EnableCors]
	[Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }
        #region SaveUserAddress
        [HttpPost("save-address")]
        public async Task<IActionResult> SaveAddress([FromBody] UserAddressRequest address)
        {
            if (address == null)
            {
                return BadRequest("Address cannot be null.");
            }

            var response = await _addressService.SaveUserAddress(address);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region GetUserAddresseDetailsByUserID
        [HttpGet("get-addresses-by-user/{userId}")]
        public async Task<IActionResult> GetAddressesByUserId(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var response = await _addressService.GetUserAddresDetailsByUserId(userId);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion
    }
}
