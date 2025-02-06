namespace SolutionTemplate.Application.Authentication
{
    public interface IAuthenticationManager
    {
        Task<bool> Authenticate(string loginId, string password, CancellationToken cancellationToken);
    }
}
