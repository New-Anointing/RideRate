using Microsoft.AspNetCore.Mvc;
using RideRate.DTOModels;
using RideRate.Helpers;
using RideRate.Models;

namespace RideRate.Services.Auth
{
    public interface ILoginServices
    {
        Task<GenericResponse<string>> Login(LoginDTO request);
        Task<GenericResponse<string>> RefreshToken();
    }
}
