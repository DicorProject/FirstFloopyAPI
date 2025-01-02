using Floppy.Application.DTOs.Request;
using Floppy.Application.Interfaces;
using Floppy.Application.Models.Request;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace FloppyAPI.Controllers
{
	[EnableCors]
	[Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISMSService _smsService;   
        public AuthController(IUserService userService,ISMSService sMSService)
        {
            _userService = userService;
            _smsService = sMSService;   
        }
        #region UserLogin
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _userService.LoginUserAsync(request);

            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response); 
        }
        #endregion

        #region UserRegister
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _userService.RegisterUserAsync(request);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response); 
        }
        #endregion

        #region UserResetPassword
        [HttpPost("passwordreset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _userService.ResetPasswordAsync(request);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        #endregion

        #region UserLogout
        [HttpPost("logout/{Id}")]
        public async Task<IActionResult> Logout(int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _userService.UserLogout(Id);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
		#endregion

		#region SendOTPToMobile
		[HttpPost("sendotp")]
		public async Task<IActionResult> sendotptomobile(string mobilenumber)
		{
			var response = await _smsService.SendOTP(mobilenumber);
			if (response.Success)
			{
				return Ok(response);
			}
			return BadRequest(response);
		}
		#endregion

		#region VerifyOtp
		[HttpPost("verifyotp")]
		public async Task<IActionResult> verifyotp(string mobilenumber,string otp)
		{
			var response = await _smsService.VerifyUser(mobilenumber,otp);
			if (response.Success)
			{
				return Ok(response);
			}
			return BadRequest(response);
		}
		#endregion
	}
}
