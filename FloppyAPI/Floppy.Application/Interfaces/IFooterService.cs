using Floppy.Application.DTOs.Response;
using Floppy.Domain.Entities;

namespace Floppy.Application.Interfaces
{
    public interface IFooterService
    {
        Task<ApiResponse<List<FooterTypeGroupDto>>> GetAllFootersAsync();
    }
}
