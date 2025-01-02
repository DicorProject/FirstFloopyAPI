using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Floppy.Infrastructure.Repositories
{
    public class AddressRepository: IAddressRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AddressRepository> _logger;    
        public AddressRepository(ApplicationDbContext context,ILogger<AddressRepository> logger)
        {
            _context = context;
            _logger = logger;   
        }
        #region SaveAddress
        //public async Task SaveAddress(AddressMasterData address)
        //{
        //    // SQL to check if an address exists with the same UserId and AddressType
        //    var exists = await _context.Addressmaster
        //        .AnyAsync(a => a.UserId == address.UserId && a.AddressType == address.AddressType);

        //    if (exists)
        //    {
        //        // If exists, perform update
        //        var updateSql = @"
        //                UPDATE AddressMaster 
        //                SET Location = @Location, City = @City, State = @State, PinCode = @PinCode, 
        //                Area = @Area, Country = @Country, StateCode = @StateCode, CountryCode = @CountryCode
        //                WHERE UserId = @UserId AND AddressType = @AddressType";

        //        await _context.Database.ExecuteSqlRawAsync(updateSql,
        //            new SqlParameter("@UserId", address.UserId),
        //            new SqlParameter("@AddressType", (object?)address.AddressType ?? DBNull.Value),
        //            new SqlParameter("@Location", (object?)address.Location ?? DBNull.Value),
        //            new SqlParameter("@City", (object?)address.City ?? DBNull.Value),
        //            new SqlParameter("@State", (object?)address.State ?? DBNull.Value),
        //            new SqlParameter("@PinCode", (object?)address.PinCode ?? DBNull.Value),
        //            new SqlParameter("@Area", (object?)address.Area ?? DBNull.Value),
        //            new SqlParameter("@Country", (object?)address.Country ?? DBNull.Value),
        //            new SqlParameter("@StateCode", (object?)address.StateCode ?? DBNull.Value),
        //            new SqlParameter("@CountryCode", (object?)address.CountryCode ?? DBNull.Value)
        //        );
        //    }
        //    else
        //    {
        //        // If not exists, perform insert
        //        var insertSql = @"
        //            INSERT INTO AddressMaster (UserId, AddressType, Location, City, State, PinCode, Area, Country, StateCode, CountryCode) 
        //            VALUES (@UserId, @AddressType, @Location, @City, @State, @PinCode, @Area, @Country, @StateCode, @CountryCode)";

        //        await _context.Database.ExecuteSqlRawAsync(insertSql,
        //            new SqlParameter("@UserId", address.UserId),
        //            new SqlParameter("@AddressType", (object?)address.AddressType ?? DBNull.Value),
        //            new SqlParameter("@Location", (object?)address.Location ?? DBNull.Value),
        //            new SqlParameter("@City", (object?)address.City ?? DBNull.Value),
        //            new SqlParameter("@State", (object?)address.State ?? DBNull.Value),
        //            new SqlParameter("@PinCode", (object?)address.PinCode ?? DBNull.Value),
        //            new SqlParameter("@Area", (object?)address.Area ?? DBNull.Value),
        //            new SqlParameter("@Country", (object?)address.Country ?? DBNull.Value),
        //            new SqlParameter("@StateCode", (object?)address.StateCode ?? DBNull.Value),
        //            new SqlParameter("@CountryCode", (object?)address.CountryCode ?? DBNull.Value)
        //        );
        //    }
        //}
		#endregion

		#region SaveUserAddress
		public async Task SaveAddress(int userId, string addressType, string location, string city,
			string state, string pinCode, string area, string country, string stateCode, string countryCode)
		{
            try
            {
                // Check if an address exists for the given UserId and AddressType
                var exists = await _context.Addressmaster
                    .AnyAsync(a => a.UserId == userId && a.AddressType == addressType);

                if (exists)
                {
                    // Update the existing address
                    var updateSql = @"
                        UPDATE AddressMaster 
                        SET Location = @Location, City = @City, State = @State, PinCode = @PinCode, 
                        Area = @Area, Country = @Country, StateCode = @StateCode, CountryCode = @CountryCode
                        WHERE UserId = @UserId AND AddressType = @AddressType";

                    await _context.Database.ExecuteSqlRawAsync(updateSql,
                        new SqlParameter("@UserId", userId),
                        new SqlParameter("@AddressType", (object?)addressType ?? DBNull.Value),
                        new SqlParameter("@Location", (object?)location ?? DBNull.Value),
                        new SqlParameter("@City", (object?)city ?? DBNull.Value),
                        new SqlParameter("@State", (object?)state ?? DBNull.Value),
                        new SqlParameter("@PinCode", (object?)pinCode ?? DBNull.Value),
                        new SqlParameter("@Area", (object?)area ?? DBNull.Value),
                        new SqlParameter("@Country", (object?)country ?? DBNull.Value),
                        new SqlParameter("@StateCode", (object?)stateCode ?? DBNull.Value),
                        new SqlParameter("@CountryCode", (object?)countryCode ?? DBNull.Value)
                    );
                }
                else
                {
                    // Insert a new address
                    var insertSql = @"
                        INSERT INTO AddressMaster (UserId, AddressType, Location, City, State, PinCode, Area, Country, StateCode, CountryCode) 
                        VALUES (@UserId, @AddressType, @Location, @City, @State, @PinCode, @Area, @Country, @StateCode, @CountryCode)";

                    await _context.Database.ExecuteSqlRawAsync(insertSql,
                        new SqlParameter("@UserId", userId),
                        new SqlParameter("@AddressType", (object?)addressType ?? DBNull.Value),
                        new SqlParameter("@Location", (object?)location ?? DBNull.Value),
                        new SqlParameter("@City", (object?)city ?? DBNull.Value),
                        new SqlParameter("@State", (object?)state ?? DBNull.Value),
                        new SqlParameter("@PinCode", (object?)pinCode ?? DBNull.Value),
                        new SqlParameter("@Area", (object?)area ?? DBNull.Value),
                        new SqlParameter("@Country", (object?)country ?? DBNull.Value),
                        new SqlParameter("@StateCode", (object?)stateCode ?? DBNull.Value),
                        new SqlParameter("@CountryCode", (object?)countryCode ?? DBNull.Value)
                    );
                }
            }
            catch(Exception ex)
            {
				_logger.LogError(ex, "An error occurred");
			}
		}
		#endregion

		#region GetAddressByUserId
		public async Task<List<AddressMasterData>> GetUserAddressByUserId(int userId)
        {
            try
            {
                var addresses = await _context.Addressmaster
                    .Where(x => x.UserId == userId)
                    .Select(x => new AddressMasterData
                    {
                        UserId = x.UserId,
                        AddressType = x.AddressType,
                        Location = x.Location,
                        City = x.City,
                        State = x.State,
                        PinCode = x.PinCode,
                        Area = x.Area,
                        Country = x.Country,
                        StateCode = x.StateCode,
                        CountryCode = x.CountryCode,
                    })
                    .ToListAsync();

                return addresses;
            }
            catch(Exception ex)
            {
				_logger.LogError(ex, "An error occurred");
				return null;
			}
        }
        #endregion
    }
}
