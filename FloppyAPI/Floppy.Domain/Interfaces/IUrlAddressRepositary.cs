using Floppy.Domain.Entities;

namespace Floppy.Domain.Interfaces
{
    public interface IUrlAddressRepositary
    {
        Task<string> GetBaseUrlAsync();
    }
}
