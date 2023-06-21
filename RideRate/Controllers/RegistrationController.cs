using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RideRate.DTOModels;
using RideRate.Helpers;
using RideRate.Models;
using RideRate.Services.Registration;
using RideRate.Utilities;

namespace RideRate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _iRegistrationService;
        public RegistrationController
        (
            IRegistrationService iRegistrationService
        )
        {
            _iRegistrationService= iRegistrationService;
        }

        [HttpPost("User")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenericResponse<ApplicationUser>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GenericResponse<ApplicationUser>))]
        [ProducesResponseType(StatusCodes.Status417ExpectationFailed, Type = typeof(GenericResponse<ApplicationUser>))]
        public async Task<ActionResult> UserRegistration(UserDTO request)
        {
            var result = await _iRegistrationService.UserRegistration(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [Authorize(Roles = $"{SD.SuperUser}, {SD.Contributors}")]
        [HttpGet("Users")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenericResponse<ApplicationUser>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GenericResponse<ApplicationUser>))]
        [ProducesResponseType(StatusCodes.Status417ExpectationFailed, Type = typeof(GenericResponse<ApplicationUser>))]
        public async Task<ActionResult> GetAllUsers()
        {
            var result = await _iRegistrationService.GetAllUsers();
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
