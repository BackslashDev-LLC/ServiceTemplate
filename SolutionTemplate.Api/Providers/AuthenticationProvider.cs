using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SolutionTemplate.Api.Configuration;
using SolutionTemplate.Application.Authentication;
using SolutionTemplate.Domain.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SolutionTemplate.Api.Providers
{
    public interface IAuthenticationProvider
    {
        Task<Result<string>> AuthenticateUser(string loginId, string password, CancellationToken cancellationToken);
    }

    internal class AuthenticationProvider : IAuthenticationProvider
    {
        private readonly IAuthenticationManager _authManager;
        private readonly AppSettings _settings;

        public AuthenticationProvider(IAuthenticationManager authManager, IOptions<AppSettings> options)
        {
            _authManager = authManager;
            _settings = options.Value;
        }

        public async Task<Result<string>> AuthenticateUser(string loginId, string password, CancellationToken cancellationToken)
        {
            if (!await _authManager.Authenticate(loginId, password, cancellationToken))
            {
                return Result.Failure<string>(new Error("Invalid", "Incorrect username or password"));
            }

            var expires = DateTime.Now.AddMinutes(_settings.ExpirationMinutes);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.JwtClientSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, loginId)
                }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _settings.JwtAudience,
                Issuer = _settings.JwtIssuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return Result.Success(jwt);
        }
    }
}
