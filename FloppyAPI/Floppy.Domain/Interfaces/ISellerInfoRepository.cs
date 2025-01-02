using Floppy.Domain.Entities;

namespace Floppy.Domain.Interfaces
{
    public interface ISellerInfoRepository
    {
        Task<VendorRegistration> FetchVendorRegistrationDetails(int Id);
    }
}
