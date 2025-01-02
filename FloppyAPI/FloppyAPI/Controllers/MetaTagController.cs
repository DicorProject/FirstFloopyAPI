using Floppy.Application.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FloppyAPI.Controllers
{
	[EnableCors]
	[Route("api/[controller]")]
    [ApiController]
    public class MetaTagController : ControllerBase
    {
        private readonly IMetaTagService _metaTagService;
        public MetaTagController(IMetaTagService metaTagService)
        {
            _metaTagService = metaTagService;   
        }
        #region GetMetagLists
        [HttpGet("GetMetagLists")]
        public async Task<IActionResult> GetMetagLists()
        {
            var response = await _metaTagService.GetMetagLists();
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region GetMetagByPageName
        [HttpGet("GetMetagByPageName/{pageName}")]
        public async Task<IActionResult> GetMetagByPageName(string pageName)
        {
            var response = await _metaTagService.GetMetagByPageName(pageName);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion
    }
}
