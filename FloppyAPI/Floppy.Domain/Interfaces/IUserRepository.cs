using Floppy.Domain.Entities;

namespace Floppy.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(Auth user);
        Task<Auth> GetByEmailAsync(string email);
        Task<Auth> GetByEmailAndMobileAsync(string email, string mobile);
        Task UpdateAsync(Auth user);
        Task<int> GetMaxIdAsync();
        Task<bool> IsEmailAndPasswordMatchAsync(string email, string password);
        Task<bool> UpdateProfileDetails(int userId, string name, string phone, string pincode, string locality, string address, string state, string city, string? image);
        Task<Auth> GetUserDetailsById(int Id);
        Task<bool> UpdateLoginStatusAndTokenByEmailAsync(string email, int loginStatus, string token);
        Task<bool> UpdateLoginStatusByIdForLogout(int UserId, int loginStatus);
        Task<Auth> GetByPhonenumberAsync(string phone);
        Task<bool> IsPhoneNoAndPasswordMatchAsync(string phone, string password);
        Task<bool> UpdateLoginStatusAndTokenByMobileNumberAsync(string mobilenumber, int loginStatus, string token);
        Task<bool> IsMobileNumberExist(string mobileno);
        Task<bool> UpdateOtpByMobileNo(string MobileNo, string Otp, DateTime OtpExpireTime);

	}
}
