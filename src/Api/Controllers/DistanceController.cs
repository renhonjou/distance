using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DistanceService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DistanceService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistanceController : Controller
    {
        private readonly ILogger<DistanceController> _logger;
        private readonly IMediator _mediator;

        public DistanceController(ILogger<DistanceController> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("calculate")]
        public async Task<IActionResult> GetDistance([FromQuery] string from, [FromQuery] string to)
        {
            if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to) || from.Length != 3 || to.Length != 3)
            {
                return BadRequest("Params aren't valid");
            }

            var query = new GetDistanceBetweenAirportsQuery(from, to);
            var result = await _mediator.Send(query);
            if (result.IsFailure)
            {
                _logger.LogError(result.Error);
            }

            return Ok(result);
        }
    }
}
