using Azure;
using RideRate.Data;
using RideRate.Helpers;
using RideRate.Models;
using System.Net;
using System.Security.Claims;

namespace RideRate.Services.UserResolver
{
    public class UserResolverService : IUserResolverService
    {
        private readonly ApiDbcontext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserResolverService(ApiDbcontext context, IHttpContextAccessor httpContextAccessor)
        {
            _context=context;
            _httpContextAccessor=httpContextAccessor;
        }

        public Guid GetUniqueAppUserId()
        {

            Guid UniqueUserId = _context.ApplicationUser.FirstOrDefault(x => x.Id == GetUserId()).AppUserId;

            return UniqueUserId;
        }

        public string GetUserId()
        {
            string claim = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                claim = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            return claim;
        }

        //Catch repoonses for errors for Location

        public GenericResponse<Location> LocationGenericResponse()
        {
            return new GenericResponse<Location>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Data = null,
                Success = false
            };
        }
        public GenericResponse<IEnumerable<Location>> LocationListGenericResponse()
        {
            return new GenericResponse<IEnumerable<Location>>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Data = null,
                Success = false
            };
        }

        //Catch responces for location ended


        //Catch repoonses for errors for Rate 

        public GenericResponse<Rate> RateGenericResponse()
        {
            return new GenericResponse<Rate>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Data = null,
                Success = false
            };
        }
        public GenericResponse<IEnumerable<Rate>> RateListGenericResponse()
        {
            return new GenericResponse<IEnumerable<Rate>>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Data = null,
                Success = false
            };
        }

        //Catch responces for Rate ended
    }
}
