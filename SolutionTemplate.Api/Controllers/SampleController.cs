using Asp.Versioning;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolutionTemplate.Application.Sample;
using SolutionTemplate.Domain.Events;
using SolutionTemplate.Domain.Sample;
using System.Net;

namespace SolutionTemplate.Api.Controllers
{
    /// <summary>
    /// Sample controller
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class SampleController : AuthenticatedController
    {
        private readonly IPublishEndpoint _publisher;
        private readonly ISampleStorage _sampleStorage;

        public SampleController(IPublishEndpoint publisher, ISampleStorage sampleStorage)
        {
            _publisher = publisher;
            _sampleStorage = sampleStorage;
        }

        /// <summary>
        /// A sample endpoint to get the logged in users ID
        /// </summary>
        /// <returns>The logged in users ID</returns>
        [ApiVersion("1.0")]
        [HttpGet("api/v1/me")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public IActionResult Me()
        {
            return Ok(LoggedInUser);
        }

        /// <summary>
        /// A sample endpoint that enqueues a message
        /// </summary>
        /// <param name="id">The id sent in the message</param>
        /// <param name="cancellationToken">A token to cancel the work</param>
        /// <returns>Accepted to indicate the message was enqueued</returns>
        [ApiVersion("1.0")]
        [HttpPost("api/v1/messages")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<IActionResult> Enqueue([FromQuery] int id, CancellationToken cancellationToken)
        {
            await _publisher.Publish(new SampleHappened(id), cancellationToken);
            return Accepted();
        }

        /// <summary>
        /// A sample endpoint that performs a database query
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the work</param>
        /// <returns>A list of items from the database</returns>
        [ApiVersion("1.0")]
        [HttpGet("api/v1/values")]
        [ProducesResponseType(typeof(List<Item>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ListValues(CancellationToken cancellationToken)
        {
            return Ok(await _sampleStorage.GetValues(cancellationToken));
        }
    }
}
