using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RideRate.DTOModels;
using RideRate.Helpers;
using RideRate.Models;
using RideRate.Services.Locations;
using RideRate.Utilities;

namespace RideRate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles =$"{SD.SuperUser}, {SD.Contributors}")]
    public class LocationController : ControllerBase
    {
        public readonly ILocationServices _IlocationServices;
        public LocationController
        (
            ILocationServices IlocationServices
        )
        {
            _IlocationServices = IlocationServices;
        }
 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenericResponse<Location>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GenericResponse<Location>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<Location>))]
        public async Task<ActionResult> CreateNewLocation(LocationDTO request)
        {
            var result = await _IlocationServices.CreateNewLocation(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("Locations")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<IEnumerable<Location>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GenericResponse<IEnumerable<Location>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<IEnumerable<Location>>))]
        public async Task<ActionResult> GetMyLocation()
        {
            var result = await _IlocationServices.GetMyLocation();
            return StatusCode((int)result.StatusCode, result);
        }

        [Authorize(Roles = $"{SD.SuperUser}, {SD.Contributors}, {SD.Consumers}")]
        [HttpGet("AllLocations")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<IEnumerable<Location>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GenericResponse<IEnumerable<Location>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<IEnumerable<Location>>))]
        public async Task<ActionResult> GetAllLocation()
        {
            var result = await _IlocationServices.GetAllLocation();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<Location>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<Location>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GenericResponse<Location>))]
        public async Task<ActionResult> UpdateLocation(Guid Id, LocationDTO request)
        {
            var result = await _IlocationServices.EditLocation(Id, request);
            return StatusCode((int)result.StatusCode, result);

        }

        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(GenericResponse<Location>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<Location>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GenericResponse<Location>))]
        public async Task<ActionResult> DeleteLocation(Guid id)
        {
            var result = await _IlocationServices.DeleteLocation(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
