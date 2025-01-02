using Floppy.Domain.Entities;

namespace Floppy.Domain.Interfaces
{
    public interface IFooterRepository
    {
        Task<List<FooterTypeGroupDto>> GetAllFootersWithParametersGroupedByTypeAsync();
    }
}
