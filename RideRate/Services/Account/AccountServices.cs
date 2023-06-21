using Microsoft.AspNetCore.Identity;
using RideRate.DTOModels;
using RideRate.Helpers;
using RideRate.Models;
using RideRate.Services.UserResolver;
using System.Net;

namespace RideRate.Services.Account
{
    public class AccountServices : IAccountServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserResolverService _iuserResolverService;
        private readonly ILogger<ChangePasswordDTO> _logger;
        private static Profile userProfile = new();
        public AccountServices
        (
            UserManager<ApplicationUser> userManager,
            IUserResolverService iuserResolverService,
            ILogger<ChangePasswordDTO> logger
        )
        {
            _userManager = userManager;
            _iuserResolverService= iuserResolverService;
            _logger = logger;
        }

        private string UserId => _iuserResolverService.GetUserId();


        public async Task<GenericResponse<string>> ChangePassword(ChangePasswordDTO request)
        {
            try
            {
                if (request.NewPassword != request.ConfirmPassword)
                {
                    return new GenericResponse<string>
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Data = null,
                        Message = $"New password and confirm password dosent match ",
                        Success = false
                    };
                }
                var user = await _userManager.FindByIdAsync(UserId);
                if (user == null)
                {
                    return new GenericResponse<string>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Data = null,
                        Message = $"Unable to load user with ID '{UserId}'",
                        Success = false
                    };
                }

                var hasPassword = await _userManager.HasPasswordAsync(user);
                if (!hasPassword)
                {
                    return new GenericResponse<string>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Data = null,
                        Message = "user has no password Redirect user to set password page",
                        Success = false
                    };
                }

                var changePasswordResult = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    return new GenericResponse<string>()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Data = null,
                        Message = "One or more input is wrong",
                        Success = false
                    };
                }

                _logger.LogInformation("User changed their password successfully.");
                return new GenericResponse<string>()
                {
                    StatusCode= HttpStatusCode.OK,
                    Data = null,
                    Message = "Your password has been changed",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<string>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = $"An error occured {ex.Message}",
                    Success = false
                };
            }
        }

        public async Task<GenericResponse<Profile>> ViewProfile()
        {
            try
            {
                var user = await _userManager.FindByIdAsync(UserId);
                if (user == null)
                {
                    return new GenericResponse<Profile>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Data = null,
                        Message = $"Unable to load user with ID '{UserId}' ",
                        Success = false
                    };
                }
                userProfile.Address = user.HomeAddress;
                userProfile.UserName = user.UserName;
                userProfile.FirstName = user.FirstName;
                userProfile.LastName = user.LastName;
                userProfile.Email = user.Email;
                userProfile.PhoneNumber = user.PhoneNumber;

                _logger.LogInformation("User profile loaded.");

                return new GenericResponse<Profile>()
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = userProfile,
                    Message = "user profile loaded successfully",
                    Success = true
                };
                
            }
            catch(Exception ex)
            {
                return new GenericResponse<Profile>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = $"An error occured {ex.Message}",
                    Success = false
                };
            }
        }
    }
}
