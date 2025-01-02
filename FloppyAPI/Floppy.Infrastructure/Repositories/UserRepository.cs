using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Floppy.Infrastructure.Repositories
{
    public class UserRepository :IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(ApplicationDbContext context,ILogger<UserRepository> logger)
        {
            _context = context; 
            _logger = logger;   
        }
		#region CreateUser
		public async Task AddAsync(Auth user)
        {
            var sql = "INSERT INTO tb_login (id,name, password, EMailId,mobileno,compid,userid,usertype) VALUES (@UserId,@Username, @Password, @Email,@MobileNo,@CompId,@UserId,@UserType)";
            await _context.Database.ExecuteSqlRawAsync(sql,
                new SqlParameter("@UserId", user.Id),
                new SqlParameter("@Username", user.Name),
                new SqlParameter("@Password", user.Password),
                new SqlParameter("@Email", user.EmailId),
                new SqlParameter("@MobileNo", user.MobileNo),
                new SqlParameter("@CompId",user.Compid),
                new SqlParameter("@UserType", "Customer")

            );
        }
		#endregion

		#region FetchUserDetailsByEmail
		public async Task<Auth> GetByEmailAsync(string email)
        {
			var user = await _context.tb_login
			   .FirstOrDefaultAsync(u => u.EmailId == email);
			return user;
        }
		#endregion

		#region FetchUserDetailsByPhoneNumber 
		public async Task<Auth>GetByPhonenumberAsync(string phone)
        {
			var user = await _context.tb_login
			   .FirstOrDefaultAsync(u => u.MobileNo == phone);
			return user;
        }
		#endregion

		#region CheckEmailAndPasswordMatch
		public async Task<bool> IsEmailAndPasswordMatchAsync(string email,string password)
        {
			var user = await _context.tb_login
			.FirstOrDefaultAsync(u => u.EmailId == email && u.Password == password);
			return user != null;
        }
		#endregion

		#region CheckPhoneNumberandPasswordMatch
		public async Task<bool> IsPhoneNoAndPasswordMatchAsync(string phone, string password)
        {
			var user = await _context.tb_login
			.FirstOrDefaultAsync(u => u.MobileNo == phone && u.Password == password);
			return user != null;
        }
		#endregion

		#region FetchUserDetailsByusingMobileNumberOrEmail
		public async Task<Auth> GetByEmailAndMobileAsync(string email, string mobile)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(mobile))
            {
                return null;
            }
			return await _context.tb_login.FirstOrDefaultAsync(u => u.EmailId == email && u.MobileNo == mobile);
		}
		#endregion

		#region UpdateLoginUserDetails
		public async Task UpdateAsync(Auth user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var sql = @"UPDATE tb_login
                                  SET name = @Name,
                                  password = @Password,
                                  EMailId = @EmailId,
                                  mobileno = @MobileNo
                                  WHERE id = @Id";

            var parameters = new[]
            {
                    new SqlParameter("@Id", user.Id),
                    new SqlParameter("@Name", user.Name),
                    new SqlParameter("@Password", user.Password),
                    new SqlParameter("@EmailId", user.EmailId),
                    new SqlParameter("@MobileNo", user.MobileNo)
            };
            await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }
		#endregion

		#region GetMaxId
		public async Task<int> GetMaxIdAsync()
        {
            var maxId = await _context.tb_login
                .Select(a => (int?)a.Id)
                .MaxAsync();
            return maxId ?? 0;
        }
		#endregion

		#region UpdateUserProfileDetails
		public async Task<bool> UpdateProfileDetails(int userId,string name,string phone,string pincode,string locality,string address,string state,string city,string? image)
        {
            var sql = @"UPDATE tb_login
                SET name = @Name,
                    mobileno = @Phone,
                    pincode = @Pincode,
                    locality = @Locality,
                    address = @Address,
                    state = @State,
                    city = @City,
                    Image = @Image
                WHERE id = @UserId";

            var parameters = new[]
            {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@Name", name ?? (object)DBNull.Value),
                new SqlParameter("@Phone", phone ?? (object)DBNull.Value),
                new SqlParameter("@Pincode", pincode ?? (object)DBNull.Value),
                new SqlParameter("@Locality", locality ?? (object)DBNull.Value),
                new SqlParameter("@Address", address ?? (object)DBNull.Value),
                new SqlParameter("@State", state ?? (object)DBNull.Value),
                new SqlParameter("@City", city ?? (object)DBNull.Value),
                new SqlParameter("@Image", image ?? (object)DBNull.Value)
            };

            int rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameters);

            return rowsAffected > 0;
        }
		#endregion

		#region GetUserDetailsById
		public async Task<Auth> GetUserDetailsById(int Id)
        {
            var user = await _context.tb_login
                                     .Where(u => u.Id == Id)
                                     .FirstOrDefaultAsync(); // Use FirstOrDefaultAsync for async operation
            return user;
        }
		#endregion

		#region CheckMobileNumberExist
		public async Task<bool> IsMobileNumberExist(string mobileno)
		{
			var exists = await _context.tb_login
									   .AnyAsync(u => u.MobileNo == mobileno); 
			return exists;
		}
		#endregion

		#region UpdateLoginStatusandTokenByEmail

		public async Task<bool> UpdateLoginStatusAndTokenByEmailAsync(string email, int loginStatus, string token)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            var sql = @"UPDATE tb_login
                        SET loginstatus = @LoginStatus,
                        token = @Token,
                        login=@Email
                        WHERE EMailId = @Email";

            var parameters = new[]
            {
                new SqlParameter("@LoginStatus", loginStatus),
                new SqlParameter("@Token", token ?? (object)DBNull.Value),
                new SqlParameter("@Email", email)
            };
            int rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
            return rowsAffected > 0;
        }
		#endregion

		#region UpdateLoginStatusByIdForLogout
		public async Task<bool> UpdateLoginStatusByIdForLogout(int userId, int loginStatus)
        {
            var sql = @"UPDATE tb_login
                SET loginstatus = @LoginStatus,
                    token = @Token
                WHERE id = @UserId";

            var parameters = new[]
            {
                new SqlParameter("@LoginStatus", loginStatus),
                new SqlParameter("@Token", DBNull.Value),
                new SqlParameter("@UserId", userId)
            };

            int rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
            return rowsAffected > 0;
        }
		#endregion

		#region UpdateLoginStatusAndTokenByMobileNumber
		public async Task<bool> UpdateLoginStatusAndTokenByMobileNumberAsync(string mobilenumber, int loginStatus, string token)
        {
            if (string.IsNullOrEmpty(mobilenumber))
            {
                throw new ArgumentNullException(nameof(mobilenumber));
            }

            var sql = @"UPDATE tb_login
                        SET loginstatus = @LoginStatus,
                        token = @Token,
                        login=@mobilenumber
                        WHERE mobileno = @mobilenumber";

            var parameters = new[]
            {
                new SqlParameter("@LoginStatus", loginStatus),
                new SqlParameter("@Token", token ?? (object)DBNull.Value),
                new SqlParameter("@mobilenumber", mobilenumber)
            };
            int rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
            return rowsAffected > 0;
        }
		#endregion

		#region UpdateOtpByMobileNo
		public async Task<bool> UpdateOtpByMobileNo(string MobileNo,string Otp,DateTime OtpExpireTime)
		{
			var sql = @"UPDATE tb_login
                SET forgototp = @OTP,
                    OtpExpireTime =@OtpExpireTime
                WHERE mobileno = @MobileNo";

			var parameters = new[]
			{
				new SqlParameter("@OTP", Otp),
                new SqlParameter("@OtpExpireTime",OtpExpireTime),
				new SqlParameter("@MobileNo", MobileNo)
			};
			int rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
			return rowsAffected > 0;
		}
		#endregion
	}
}
