using System.Data;

namespace SolutionTemplate.Application.Abstractions.Data
{
    public interface ISqlConnectionFactory
    {
        Task<IDbConnection> CreateConnection(string databaseKey, CancellationToken cancellationToken);
    }
}
