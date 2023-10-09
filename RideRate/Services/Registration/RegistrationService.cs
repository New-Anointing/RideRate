using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RideRate.Data;
using RideRate.DTOModels;
using RideRate.Helpers;
using RideRate.Models;
using RideRate.Utilities;
using System.Net;

namespace RideRate.Services.Registration
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ApiDbcontext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private static ApplicationUser user = new();
        public RegistrationService
        (
            ApiDbcontext context,
            UserManager<ApplicationUser> userManager
        )
        {
            _context = context;
            _userManager = userManager;
        }

        private int CreateCode()
        {
            Random code = new Random();
            return code.Next(6);
        }

        public async Task<GenericResponse<ApplicationUser>> UserRegistration(UserDTO request)
        {
            try
            {
                var userExist = await _userManager.FindByEmailAsync(request.Email);
                if (userExist is null)
                {
                    user.Id = Guid.NewGuid().ToString();
                    user.Email = request.Email;
                    user.UserName = request.Email;
                    user.FirstName = request.FirstName;
                    user.LastName = request.LastName;
                    user.HomeAddress = request.Address;
                    user.PhoneNumber = request.PhoneNumber;
                    user.AppUserId = Guid.NewGuid();
                    user.Role = request.Role.ToString();
                    user.Gender = request.Gender.ToString();
                    //CREATE VERIFICATION CODE
                    user.VerificationToken = CreateCode();

                    var result = await _userManager.CreateAsync(user, request.Password);

                    if (!result.Succeeded)
                    {
                        return new GenericResponse<ApplicationUser>
                        {
                            StatusCode = HttpStatusCode.ExpectationFailed,
                            Data = null,
                            Message = "Something went wrong :(" +
                            "Check and validate all inputs",
                            Success = false
                        };
                    }
                    else
                    {
                        //ASSINGING ROLES
                        if (request.Role.ToString() == SD.Contributors)
                        {
                            await _userManager.AddToRoleAsync(user, SD.Contributors);
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, SD.Consumers);
                        }
                    }

                    return new GenericResponse<ApplicationUser>
                    {
                        StatusCode = HttpStatusCode.Created,
                        Data = user,
                        Message = "User is created successfully",
                        Success = true
                    };
                }

                return new GenericResponse<ApplicationUser>
                {
                    StatusCode = HttpStatusCode.ExpectationFailed,
                    Data = null,
                    Message = "User with this email already exist",
                    Success = false
                };
            }
            catch( Exception ex )
            {
                return new GenericResponse<ApplicationUser>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = $"An error occured {ex.Message}",
                    Success = false
                };
            }
        }
        public async Task<GenericResponse<IEnumerable<ApplicationUser>>> GetAllUsers()
        {
            try
            {
                var users = await _context.ApplicationUser.ToListAsync();
                return new GenericResponse<IEnumerable<ApplicationUser>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = users,
                    Message = "Data Loaded successfully",
                    Success = true
                };
            }
            catch(Exception ex)
            {
                return new GenericResponse<IEnumerable<ApplicationUser>>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = $"An error occurred {ex.Message}",
                    Success= false
                };
            }
        }
    }
}
