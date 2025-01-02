using Floppy.Application.DTOs.Request;
using Floppy.Application.DTOs.Response;
using Floppy.Application.Interfaces;
using Floppy.Application.Models.Request;
using Floppy.Application.Models.Response;
using Floppy.Domain.Entities;
using Floppy.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Floppy.Application.Services
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;   
        private readonly ILogger<UserService> _logger;  
        public UserService(IUserRepository userRepository,IConfiguration configuration,IEmailService emailService,ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration; 
            _emailService = emailService;  
            _logger = logger;   
        }
        #region RegisterUser
        public async Task<ApiResponse<UserCreationResponse>> RegisterUserAsync(RegisterUserRequest request)
        {
            try
            {
                var existingphonenumber = await _userRepository.GetByPhonenumberAsync(request.MobileNumber);
                if (existingphonenumber != null)
                {
                    return new ApiResponse<UserCreationResponse>
                    {
                        Success = false,
                        Message = "Mobile Number is already in use.",
                        Data = null
                    };
                }
                var existingUser = await _userRepository.GetByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return new ApiResponse<UserCreationResponse>
                    {
                        Success = false,
                        Message = "Email address is already in use.",
                        Data = null
                    };
                }
                var maxId = await _userRepository.GetMaxIdAsync();
                var user = new Auth
                {
                    Id = maxId + 1,
                    Name = $"{request.FirstName} {request.LastName}",
                    EmailId = request.Email,
                    Password = request.Password,
                    MobileNo = request.MobileNumber,
                    UserType = "vendor",
                    Compid=1,
                };
                await _userRepository.AddAsync(user);
                var token = GenerateJwtToken(user);
                var response = new UserCreationResponse
                {
                    UserId = user.Id,
                    Token=token
                };
                _emailService.SendEmailAsync(user.EmailId,request.FirstName, "Registration",0);
                return new ApiResponse<UserCreationResponse>
                {
                    Success = true,
                    Message = $"{request.FirstName} registered successfully",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserCreationResponse>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null
                };
            }
        }
        #endregion

        #region LoginUser
        public async Task<ApiResponse<UserLoginResponse>> LoginUserAsync(LoginUserRequest request)
        {
            try
            {
                bool isMatch = false;
                Auth user = null;

                if (!string.IsNullOrEmpty(request.Email))
                {
                    // Check if email and password match using raw SQL
                    isMatch = await _userRepository.IsEmailAndPasswordMatchAsync(request.Email, request.Password);

                    if (isMatch)
                    {
                        // Retrieve user info by email
                        user = await _userRepository.GetByEmailAsync(request.Email);
                    }
                    else
                    {
                        return new ApiResponse<UserLoginResponse>
                        {
                            Success = false,
                            Message = "Invalid email or password",
                            Data = null
                        };
                    }
                }
                else if (!string.IsNullOrEmpty(request.MobileNumber))
                {
                    // Check if MobileNumber and password match using raw SQL
                    isMatch = await _userRepository.IsPhoneNoAndPasswordMatchAsync(request.MobileNumber, request.Password);

                    if (isMatch)
                    {
                        // Retrieve user info by MobileNumber
                        user = await _userRepository.GetByPhonenumberAsync(request.MobileNumber);
                    }
                    else
                    {
                        return new ApiResponse<UserLoginResponse>
                        {
                            Success = false,
                            Message = "Invalid phone number or password",
                            Data = null
                        };
                    }
                }

                // Generate JWT token
                var token = GenerateJwtToken(user);
                var response = new UserLoginResponse
                {
                    UserId = user.Id,
                    UserName= user.Name,    
                    Email = user.EmailId,
                    MobileNo= user.MobileNo,    
                    Token = token
                };
                if (!string.IsNullOrEmpty(request.Email))
                {

                    await _userRepository.UpdateLoginStatusAndTokenByEmailAsync(request.Email, 1, token);
                }
                else
                {
                    await _userRepository.UpdateLoginStatusAndTokenByMobileNumberAsync(request.MobileNumber,1, token);  
                }
                return new ApiResponse<UserLoginResponse>
                {
                    Success = true,
                    Message = $"Welcome back, {user.Name}!",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserLoginResponse>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null
                };
            }
        }
        #endregion

        #region ResetPassword
        public async Task<ApiResponse<bool>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            try
            {
                // Validate request model
                if (string.IsNullOrEmpty(request.Mobile) || string.IsNullOrEmpty(request.Password))
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "All fields are required.",
                        Data = false
                    };
                }
                // Verify user
                var user = await _userRepository.GetByPhonenumberAsync(request.Mobile);
                if (user == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "User not found.",
                        Data = false
                    };
                }
                user.Password = request.Password;
                await _userRepository.UpdateAsync(user);
                _emailService.SendEmailAsync(user.EmailId, null, "PasswordReset",0);
                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Password reset successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}",
                    Data = false
                };
            }
        }

        #endregion

        #region Profileupdate
        public async Task<ApiResponse<bool>> UpdateProfileDetails(ProfileUpdateRequest request)
        {
            var response = new ApiResponse<bool>();

            try
            {
                // Validate the input
                if (request == null || request.UserId <= 0)
                {
                    response.Success = false;
                    response.Message = "Invalid request data.";
                    response.Data = false; // Ensure Data is set in all cases
                    return response;
                }

                // Update the user profile in the database
                var result = await _userRepository.UpdateProfileDetails(
                    request.UserId,
                    request.Name,
                    request.Phone,
                    request.Pincode,
                    request.Locality,
                    request.Address,
                    request.State,
                    request.City,
                    request.Image
                );

                if (!result)
                {
                    response.Success = false;
                    response.Message = "User not found or update failed.";
                    response.Data = false;
                    return response;
                }

                response.Success = true;
                response.Message = "Profile updated successfully.";
                response.Data = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                response.Data = false;
            }

            return response;
        }
        #endregion

        #region ProfileDeatailsById
        public async Task<ApiResponse<Auth>> GetProfileDetailsById(int id)
        {
            var response = new ApiResponse<Auth>();
            try
            {
                var profile =await  _userRepository.GetUserDetailsById(id);

                if (profile != null)
                {
                    response.Success = true;
                    response.Message = "Profile details retrieved successfully.";
                    response.Data = profile;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Profile not found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred while retrieving the profile details: {ex.Message}";
            }

            return response;
        }
        #endregion

        #region Logout
        public async Task<ApiResponse<bool>> UserLogout(int userId)
        {
            var response = new ApiResponse<bool>();
            try
            {
                // Attempt to update the login status for the specified user ID
                bool result = await _userRepository.UpdateLoginStatusByIdForLogout(userId, 0);

                if (result)
                {
                    response.Success = true;
                    response.Message = "Logout successful.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "User not found or logout failed.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred during logout: {ex.Message}";
            }

            return response;
        }
        #endregion

        #region GenerateJwtToken
        private string GenerateJwtToken(Auth user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.EmailId),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        #endregion

    }
}
