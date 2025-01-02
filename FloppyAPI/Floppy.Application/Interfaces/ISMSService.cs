using Floppy.Application.DTOs.Response;

namespace Floppy.Application.Interfaces
{
	public interface ISMSService
	{
		Task<ApiResponse<bool>> SendOTP(string mobileno);
		Task<ApiResponse<bool>> VerifyUser(string mobileNo, string otp);
		Task SendSMSForOrderConfirmation(string TemplateName, int UserId);
		Task SendSMSForEnquiry(string TemplateName, int UserId, string serviceName);
		Task SendSMSForClientMsg(string TemplateName, int UserId);
		Task SendSMSForOrderReschedule(string TemplateName, int UserId);
	}
}
