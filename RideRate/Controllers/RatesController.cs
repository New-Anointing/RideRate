using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RideRate.DTOModels;
using RideRate.Helpers;
using RideRate.Models;
using RideRate.Services.Rates;
using RideRate.Utilities;

namespace RideRate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{SD.SuperUser}, {SD.Contributors}")]
    public class RatesController : ControllerBase
    {
        private readonly IRatesServices _irateServices;
        public RatesController
        (
            IRatesServices irateServices
        )
        {
            _irateServices = irateServices;
        }

        [HttpPost("Rate/{locationId}")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GenericResponse<Rate>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<Rate>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GenericResponse<Rate>))]
        public async Task<ActionResult> Create(Guid locationId, RateDTO request)
        {
            var result = await _irateServices.Create(locationId, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("Approved")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<IEnumerable<Rate>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<IEnumerable<Rate>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GenericResponse<IEnumerable<Rate>>))]
        public async Task<ActionResult> GetAllRates()
        {
            var result = await _irateServices.GetAllRates();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("UserRates")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<IEnumerable<Rate>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<IEnumerable<Rate>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GenericResponse<IEnumerable<Rate>>))]
        public async Task<ActionResult> UserRates()
        {
            var result = await _irateServices.UserRates();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<IEnumerable<Rate>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<IEnumerable<Rate>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GenericResponse<IEnumerable<Rate>>))]
        public async Task<ActionResult> Rates()
        {
            var result = await _irateServices.Rates();
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpGet("PendingApproval")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<IEnumerable<Rate>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<IEnumerable<Rate>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GenericResponse<IEnumerable<Rate>>))]
        public async Task<ActionResult> SentRates()
        {
            var result = await _irateServices.SentRates();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("Edit")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<Rate>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<Rate>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GenericResponse<Rate>))]
        public async Task<ActionResult> Edit(Guid Id, RateDTO request)
        {
            var result = await _irateServices.Edit(Id, request);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<Rate>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<Rate>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GenericResponse<Rate>))]
        public async Task<ActionResult> Delete(Guid Id)
        {
            var result = await _irateServices.Delete(Id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("SendForApproval")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<IEnumerable<Rate>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(GenericResponse<IEnumerable<Rate>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GenericResponse<IEnumerable<Rate>>))]
        public async Task<ActionResult> SendForApproval()
        {
            var result = await _irateServices.SendForApproval();
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpPost("ApproveRate")]
        [Authorize(Roles = $"{SD.SuperUser}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericResponse<Rate>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(GenericResponse<Rate>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(GenericResponse<Rate>))]
        public async Task<ActionResult> ApproveRate(Guid Id, ApprovalStatusDTO request)
        {
            var result = await _irateServices.ApproveRate(Id, request);
            return StatusCode((int)result.StatusCode, result);
        }


    }
}
