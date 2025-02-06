using SolutionTemplate.Application.Authentication;

namespace SolutionTemplate.Infrastructure.Authentication
{
    internal class SampleAuthManager : IAuthenticationManager
    {
        public Task<bool> Authenticate(string loginId, string password, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(loginId) || string.IsNullOrWhiteSpace(password))
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
    }
}
