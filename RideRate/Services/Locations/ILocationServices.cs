using RideRate.DTOModels;
using RideRate.Helpers;
using RideRate.Models;

namespace RideRate.Services.Locations
{
    public interface ILocationServices
    {
        Task<GenericResponse<Location>> CreateNewLocation(LocationDTO request);
        Task<GenericResponse<IEnumerable<Location>>> GetAllLocation();
        Task<GenericResponse<IEnumerable<Location>>> GetMyLocation();
        Task<GenericResponse<Location>> EditLocation(Guid id, LocationDTO request);
        Task<GenericResponse<Location>> DeleteLocation(Guid id);
    }
}
