using Floppy.Application.DTOs.Response;
using Floppy.Application.Models.Request;
using Floppy.Domain.Entities;

namespace Floppy.Application.Interfaces
{
    public interface IAddressService
    {
        Task<ApiResponse<string>> SaveUserAddress(UserAddressRequest address);
        Task<ApiResponse<List<AddressMasterData>>> GetUserAddresDetailsByUserId(int userId);
    }
}
