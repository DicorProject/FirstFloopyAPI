using Floppy.Application.DTOs.Response;
using Floppy.Domain.Entities;

namespace Floppy.Application.Interfaces
{
    public interface IMetaTagService
    {
        Task<ApiResponse<List<Metatag>>> GetMetagLists();
        Task<ApiResponse<Metatag>> GetMetagByPageName(string pageName);
    }
}
