namespace SolutionTemplate.Api.Models
{
    /// <summary>
    /// An object representing a Login Request
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// The user's Login Id (for example an email address)
        /// </summary>
        public string LoginId { get; set; } = string.Empty;
        /// <summary>
        /// The user's password
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
