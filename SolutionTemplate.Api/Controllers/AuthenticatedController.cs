using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SolutionTemplate.Api.Controllers
{
    /// <summary>
    /// Base controller for use in controllers where authentication is required
    /// </summary>
    public class AuthenticatedController : Controller
    {
        /// <summary>
        /// The identity of the logged in user
        /// </summary>
        public string LoggedInUser
        {
            get
            {
                return this.User.Claims.First(c => c.Type == ClaimTypes.Name).Value;
            }
        }
    }
}
