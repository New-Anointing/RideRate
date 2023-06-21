using RideRate.Helpers;
using RideRate.Models;
using System.Security.Claims;

namespace RideRate.Services.UserResolver
{
    public interface IUserResolverService
    {
        public Guid GetUniqueAppUserId();
        public string GetUserId();
        public GenericResponse<Location> LocationGenericResponse();
        public GenericResponse<IEnumerable<Location>> LocationListGenericResponse();
        public GenericResponse<Rate> RateGenericResponse();
        public GenericResponse<IEnumerable<Rate>> RateListGenericResponse();
    }
}
