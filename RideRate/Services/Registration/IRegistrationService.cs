using RideRate.DTOModels;
using RideRate.Helpers;
using RideRate.Models;

namespace RideRate.Services.Registration
{
    public interface IRegistrationService
    {
        Task<GenericResponse<ApplicationUser>> UserRegistration(UserDTO request);
        Task<GenericResponse<IEnumerable<ApplicationUser>>> GetAllUsers();
    }
}
