using Microsoft.EntityFrameworkCore;
using RideRate.Data;
using RideRate.DTOModels;
using RideRate.Helpers;
using RideRate.Models;
using RideRate.Services.UserResolver;
using System.Net;

namespace RideRate.Services.Locations
{
    public class LocationServices : ILocationServices
    {
        private readonly ApiDbcontext _context;
        private readonly IUserResolverService _userResolverService;
        private Location location = new(); 

        public LocationServices(ApiDbcontext context, IUserResolverService userResolverService)
        {
            _context=context;
            _userResolverService=userResolverService;
        }

        private GenericResponse<Location> GenericResponse => _userResolverService.LocationGenericResponse();
        private GenericResponse<IEnumerable<Location>> ListGenericResponse => _userResolverService.LocationListGenericResponse();
        private Guid appUserId => _userResolverService.GetUniqueAppUserId();

        public async Task<GenericResponse<Location>> CreateNewLocation(LocationDTO request)
        {
            try
            {
                var locationExist = await _context.Location.FirstOrDefaultAsync(l => l.Start.ToLower() == request.Start.ToLower() && l.Destination.ToLower() == request.Destination.ToLower());
                if (locationExist is null) 
                { 
                    location.Start = request.Start;
                    location.Destination = request.Destination;
                    location.AppUserId = appUserId;
                    location.Description = request.Description;

                    _context.Location.Add(location);
                    await _context.SaveChangesAsync();

                    return new GenericResponse<Location>
                    {
                        StatusCode = HttpStatusCode.Created,
                        Data = location,
                        Message = "Location created successfully",
                        Success = true
                    };
                }

                return new GenericResponse<Location>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Message = "Location already exist",
                    Success = false
                };
            }
            catch(Exception ex)
            {
                GenericResponse.Message =  $"An error occured {ex.Message}";
                return GenericResponse;
            }
        }

        public async Task<GenericResponse<IEnumerable<Location>>> GetAllLocation()
        {
            try
            {
                var locations = await _context.Location.ToListAsync();
                if(locations.Any())
                {
                    return new GenericResponse<IEnumerable<Location>>
                    {
                        StatusCode = HttpStatusCode.OK,
                        Data = locations,
                        Message = "Data loded successfully",
                        Success = true
                    };
                }
                return new GenericResponse<IEnumerable<Location>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Data = null,
                    Message = "No location Has been created",
                    Success = true
                };
            }
            catch(Exception ex)
            {
                ListGenericResponse.Message =  $"An error occured {ex.Message}";
                return ListGenericResponse;
            }
        }

        public async Task<GenericResponse<IEnumerable<Location>>> GetMyLocation()
        {
            try
            {
                var mylocation = await _context.Location.Where(l => l.AppUserId == appUserId).ToListAsync();
                if (mylocation.Any())
                {
                    return new GenericResponse<IEnumerable<Location>>
                    {
                        StatusCode = HttpStatusCode.OK,
                        Data = mylocation,
                        Message = "Data loaded successfully",
                        Success = true
                    };
                }

                return new GenericResponse<IEnumerable<Location>>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Data = null,
                    Message = "No location Has been created",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                ListGenericResponse.Message =  $"An error occured {ex.Message}";
                return ListGenericResponse;
            }
        }

        public async Task<GenericResponse<Location>> EditLocation(Guid id, LocationDTO request)
        {
            try
            {
                var location_to_edit = await _context.Location.FirstOrDefaultAsync(l=>l.Id == id && l.AppUserId == appUserId);
                if(location_to_edit != null)
                {
                    location_to_edit.Start = request.Start;
                    location_to_edit.Destination = request.Destination;
                    location_to_edit.DateUpdated = DateTime.Now;
                    location_to_edit.DateModified = DateTime.Now;
                    location_to_edit.Description = request.Description;

                    _context.Update(location_to_edit);
                    await _context.SaveChangesAsync();

                    return new GenericResponse<Location>
                    {
                        Success = true,
                        Data = location_to_edit,
                        Message = "Location Updated successfully",
                        StatusCode = HttpStatusCode.OK
                    };
                    
                }

                return new GenericResponse<Location>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Data = null,
                    Message = "No such location is found",
                    Success = false
                };

            }
            catch(Exception ex) 
            {
                GenericResponse.Message =  $"An error occured {ex.Message}";
                return GenericResponse;
            }
        }

        public async Task<GenericResponse<Location>> DeleteLocation(Guid id)
        {
            try
            {
                var location_to_delete = await _context.Location.FirstOrDefaultAsync(l => l.Id == id);
                var location_in_use = await _context.Rate.FirstOrDefaultAsync(l => l.Location.Id == id);
                if (location_in_use != null)
                {
                    return new GenericResponse<Location>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Data = null,
                        Message = "location cannot be deleted because location is in use",
                        Success = false
                    };
                }
                else if(location_to_delete != null )
                {
                    _context.Remove(location_to_delete);
                    await _context.SaveChangesAsync();
                    return new GenericResponse<Location>
                    {
                        StatusCode = HttpStatusCode.NoContent,
                        Data = null,
                        Message = "Location Deleted Successfully",
                        Success = true
                    };
                }

                return new GenericResponse<Location>
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Data = null,
                    Message = "no such location was found",
                    Success = false
                };
            }
            catch(Exception ex)
            {
                GenericResponse.Message = $"An error occured {ex.Message}";
                return GenericResponse;
            }
        }

    }
}
