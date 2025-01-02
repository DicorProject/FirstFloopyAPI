using Floppy.Application.DTOs.Request;
using Floppy.Application.DTOs.Response;
using Floppy.Application.Models.Request;
using Floppy.Application.Models.Response;
using Floppy.Domain.Entities;

namespace Floppy.Application.Interfaces
{
    public interface IUserService
    {
        Task<ApiResponse<UserCreationResponse>> RegisterUserAsync(RegisterUserRequest request);
        Task<ApiResponse<UserLoginResponse>> LoginUserAsync(LoginUserRequest request);
        Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordRequest request);
        Task<ApiResponse<bool>> UpdateProfileDetails(ProfileUpdateRequest request);
        Task<ApiResponse<Auth>> GetProfileDetailsById(int id);
        Task<ApiResponse<bool>> UserLogout(int userId);
    }
}
