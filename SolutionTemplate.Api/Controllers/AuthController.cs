using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolutionTemplate.Api.Models;
using SolutionTemplate.Api.Providers;
using System.Net;

namespace SolutionTemplate.Api.Controllers
{
    /// <summary>
    /// Controller used for authentication purposes
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly IAuthenticationProvider _authProvider;

        public AuthController(IAuthenticationProvider authProvider)
        {
            _authProvider = authProvider;
        }

        /// <summary>
        /// Endpoint for authenticating a user to the API
        /// </summary>
        /// <param name="request">The object representing the login request</param>
        /// <param name="cancellationToken">A token to cancel the work</param>
        /// <returns>A JWT for use in authorized operations, or an error</returns>
        [ApiVersion("1.0")]
        [HttpPost("api/v1/login")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Login([FromBody]LoginRequest request, CancellationToken cancellationToken)
        {
            var loginResult = await _authProvider.AuthenticateUser(request.LoginId, request.Password, cancellationToken);

            if (!loginResult.IsSuccess)
            {
                return Unauthorized(loginResult.Error.Description);
            }

            return Ok(loginResult.Value);
        }
    }
}
