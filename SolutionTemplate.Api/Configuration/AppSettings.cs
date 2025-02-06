namespace SolutionTemplate.Api.Configuration
{
    internal class AppSettings
    {
        public string JwtClientSecret { get; set; } = string.Empty;
        public string JwtAudience { get; set; } = string.Empty;
        public string JwtIssuer { get; set; } = string.Empty;
        public int ExpirationMinutes { get; set; }
    }
}
