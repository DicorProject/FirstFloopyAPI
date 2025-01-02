using Floppy.Domain.Entities;

namespace Floppy.Domain.Interfaces
{
    public interface IAddressRepository
    {
        //Task SaveAddress(AddressMasterData address);
        Task SaveAddress(int userId, string addressType, string location, string city,
            string state, string pinCode, string area, string country, string stateCode, string countryCode);

		Task<List<AddressMasterData>> GetUserAddressByUserId(int userId);
    }
}
