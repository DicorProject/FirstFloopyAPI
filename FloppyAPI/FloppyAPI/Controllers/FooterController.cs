using Floppy.Application.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FloppyAPI.Controllers
{
	[EnableCors]
	[Route("api/[controller]")]
    [ApiController]
    public class FooterController : ControllerBase
    {
        private readonly IFooterService _footerService;
        public FooterController(IFooterService footerService)
        {
            _footerService= footerService;
        }
        #region FetchFooterDataList
        [HttpGet("fetch-listData-footer")]
        public async Task<IActionResult> FetchListOfDataOfFooter()
        {
            var response = await _footerService.GetAllFootersAsync();

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion
    }
}
