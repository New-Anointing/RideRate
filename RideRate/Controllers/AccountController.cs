using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RideRate.DTOModels;
using RideRate.Helpers;
using RideRate.Services.Account;

namespace RideRate.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServices _accountServices;

        public AccountController
        (
            IAccountServices accountServices
        )
        {
            _accountServices= accountServices;
        }

        [HttpPost("ChangePassword")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GenericResponse<string>))]
        public async Task<ActionResult> Changepassword(ChangePasswordDTO request)
        {
            var result = await _accountServices.ChangePassword(request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("Profile")]
        [ProducesResponseType(StatusCodes.Status200OK, Type=typeof(GenericResponse<Profile>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type=typeof(GenericResponse<Profile>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type=typeof(GenericResponse<Profile>))]
        public async Task<ActionResult> ViewProfile()
        {
            var result = await _accountServices.ViewProfile();
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
