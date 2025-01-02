using Floppy.Application.Interfaces;
using Floppy.Domain.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FloppyAPI.Controllers
{
	[EnableCors]
	[Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;  
        public CartController(ICartService cartService)
        {
            _cartService = cartService; 
        }
        #region GetCartItemsByUserId
        [HttpGet("cart-items/{userId}")]
        public async Task<IActionResult> GetCartItemsByUser(int userId)
        {
            var response = await _cartService.GetCartDetailsAsync(userId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region Add Cart Item
        [HttpPost("add-cart-items")]
        public async Task<IActionResult> AddCartItems([FromBody] List<CartItemDTO> cartmasterDTOs)
        {
            if (!ModelState.IsValid || cartmasterDTOs == null || !cartmasterDTOs.Any())
            {
                return BadRequest(ModelState);
            }
            var response = await _cartService.AddCartDetails(cartmasterDTOs);

            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        #endregion

        #region Update Cart Item
        [HttpPost("update-cart-items")]
        public async Task<IActionResult> UpdateCartItems([FromBody] List<CartItemDTO> cartmasterDTOs)
        {
            var response = await _cartService.UpdateCartDetails(cartmasterDTOs);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        #endregion

        #region Delete Cart Item
        [HttpPost("delete-cart-item")]
        public async Task<IActionResult> DeleteCartItem(List<int> CardItemIds)
        {
            var response = await _cartService.DeleteCartAsync(CardItemIds);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion
    }
}
