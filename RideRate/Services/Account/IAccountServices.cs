
using RideRate.DTOModels;
using RideRate.Helpers;

namespace RideRate.Services.Account
{
    public interface IAccountServices
    {
        Task<GenericResponse<string>> ChangePassword(ChangePasswordDTO request);
        Task<GenericResponse<Profile>> ViewProfile();
    }
}