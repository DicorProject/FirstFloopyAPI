using Floppy.Application.DTOs.Response;
using Floppy.Application.Interfaces;
using Floppy.Domain.Interfaces;
using System.Net;

namespace Floppy.Application.Services
{
	public  class SMSService : ISMSService
	{
        private readonly IUserRepository _userRepository;
		private readonly IEmailRepository _emailRepository;
        public SMSService(IUserRepository userRepository,IEmailRepository emailRepository)
        {
            _userRepository=userRepository;
			_emailRepository=emailRepository;	
        }
		#region SendOTP
		public async Task<ApiResponse<bool>> SendOTP(string mobileno)
		{
			try
			{
				string formattedMobileNo = "+91" + mobileno;
				bool isMobileNumberExist = await _userRepository.IsMobileNumberExist(formattedMobileNo);

				if (isMobileNumberExist)
				{
					var random = new Random();
					int otp = random.Next(100000, 999999);

					bool isOtpUpdated = await _userRepository.UpdateOtpByMobileNo(formattedMobileNo, otp.ToString(),DateTime.UtcNow);

					if (isOtpUpdated)
					{
						var smsdata = await _emailRepository.GetMobileSMSTemplate("OTP");
						string TemplateId = smsdata.TemplateID;
						string msgnew = smsdata.TemplateContent;
						msgnew = msgnew.Replace("{#var#}", otp.ToString());
						SendSMSToMobile(mobileno, msgnew,TemplateId);
						return new ApiResponse<bool>
						{
							Success = true,
							Message = "OTP sent successfully.",
							Data = true
						};
					}
					else
					{
						return new ApiResponse<bool>
						{
							Success = false,
							Message = "Failed to update OTP.",
							Data = false
						};
					}
				}
				else
				{
					return new ApiResponse<bool>
					{
						Success = false,
						Message = "Mobile number not found.",
						Data = false
					};
				}
			}
			catch (Exception ex)
			{
				return new ApiResponse<bool>
				{
					Success = false,
					Message = $"An error occurred: {ex.Message}",
					Data = false
				};
			}
		}
		#endregion

		#region VerifyUser
		public async Task<ApiResponse<bool>> VerifyUser(string mobileNo, string otp)
		{
			string formattedMobileNo = "+91" + mobileNo;
			var userData = await _userRepository.GetByPhonenumberAsync(formattedMobileNo);

			if (userData == null)
			{
				return new ApiResponse<bool> { Success = false, Message = "User not found." };
			}

			DateTime? otpExpireTime = userData.OtpExpireTime;
			DateTime currentTime = DateTime.UtcNow;

			if ((currentTime - otpExpireTime.Value).TotalMinutes <= 10)
			{
				bool isValidOtp = userData.ForgotOtp == otp;
				return new ApiResponse<bool>
				{
					Success = isValidOtp,
					Message = isValidOtp ? "OTP is valid." : "Invalid OTP."
				};
			}

			return new ApiResponse<bool> { Success = false, Message = "OTP has expired." };
		}
		#endregion

		#region SendSMSForOrderConformation 
		public async Task SendSMSForOrderConfirmation(string TemplateName, int UserId)
		{
			var UserDetails = await _userRepository.GetUserDetailsById(UserId);
			if (UserDetails == null)
			{
				throw new Exception("User not found");
			}

			string MobileNo = UserDetails.MobileNo;
			string UserName = UserDetails.Name;

			var smsdata = await _emailRepository.GetMobileSMSTemplate(TemplateName);
			if (smsdata != null)
			{
				string TemplateId = smsdata.TemplateID;
				string msgnew = smsdata.TemplateContent;
				msgnew = msgnew.Replace("{#var#}", UserName);
				SendSMSToMobile(MobileNo, msgnew, TemplateId);
			}
			else
			{
				throw new Exception("SMS template not found");
			}
		}

		#endregion

		#region SendSMSForEnquiry
		public async Task SendSMSForEnquiry(string TemplateName, int UserId,string serviceName)
		{
			var UserDetails = await _userRepository.GetUserDetailsById(UserId);
			if (UserDetails == null)
			{
				throw new Exception("User not found");
			}

			string MobileNo = UserDetails.MobileNo;
			string UserName = UserDetails.Name;

			var smsdata = await _emailRepository.GetMobileSMSTemplate(TemplateName);
			if (smsdata != null)
			{
				string TemplateId = smsdata.TemplateID;
				string msgnew = smsdata.TemplateContent;
				msgnew = msgnew.Replace("{#var#}", UserName);
				msgnew = msgnew.Replace("{#var2#}", serviceName);
				SendSMSToMobile(MobileNo, msgnew, TemplateId);
			}
			else
			{
				throw new Exception("SMS template not found");
			}
		}

		#endregion

		#region SendSMSForClientMsg
		public async Task SendSMSForClientMsg(string TemplateName, int UserId)
		{
			var UserDetails = await _userRepository.GetUserDetailsById(UserId);
			if (UserDetails == null)
			{
				throw new Exception("User not found");
			}

			string MobileNo = UserDetails.MobileNo;
			string UserName = UserDetails.Name;

			var smsdata = await _emailRepository.GetMobileSMSTemplate(TemplateName);
			if (smsdata != null)
			{
				string TemplateId = smsdata.TemplateID;
				string msgnew = smsdata.TemplateContent;
				msgnew = msgnew.Replace("{#var#}", UserName);
				SendSMSToMobile(MobileNo, msgnew, TemplateId);
			}
			else
			{
				throw new Exception("SMS template not found");
			}
		}

		#endregion

		#region SendSMSForOrderReschedule
		public async Task SendSMSForOrderReschedule(string TemplateName, int UserId)
		{
			var UserDetails = await _userRepository.GetUserDetailsById(UserId);
			if (UserDetails == null)
			{
				throw new Exception("User not found");
			}

			string MobileNo = UserDetails.MobileNo;
			string UserName = UserDetails.Name;

			var smsdata = await _emailRepository.GetMobileSMSTemplate(TemplateName);
			if (smsdata != null)
			{
				string TemplateId = smsdata.TemplateID;
				string msgnew = smsdata.TemplateContent;
				msgnew = msgnew.Replace("{#var#}", UserName);
				SendSMSToMobile(MobileNo, msgnew, TemplateId);
			}
			else
			{
				throw new Exception("SMS template not found");
			}
		}

		#endregion

		#region SendSMSToMobile
		private void SendSMSToMobile(string MobileNo, string Sms, string tid)
		{

			string Message = "http://sms.yoursmsbox.com/api/sendhttp.php?authkey=34316c6f70707934343294&mobiles=#MobileNo#&message=#sms#&sender=FFIRST&route=2&country=91&DLT_TE_ID=#tid#";
			Message = Message.Replace("#MobileNo#", MobileNo);
			Message = Message.Replace("#sms#", Sms);
			Message = Message.Replace("#tid#", tid);
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Message);
			req.Method = "GET";
			req.GetResponse();
		}
		#endregion
	}
}
