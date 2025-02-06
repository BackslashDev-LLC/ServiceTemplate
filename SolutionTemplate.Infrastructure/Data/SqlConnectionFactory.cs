using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using SolutionTemplate.Application.Abstractions.Data;
using SolutionTemplate.Infrastructure.Configuration;
using System.Data;

namespace SolutionTemplate.Infrastructure.Data
{
    internal sealed class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly DatabaseConfig _config;

        public SqlConnectionFactory(IOptions<DatabaseConfig> config)
        {
            _config = config.Value ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<IDbConnection> CreateConnection(string databaseKey, CancellationToken cancellationToken)
        {
            if (!_config.ConnectionStrings.TryGetValue(databaseKey, out var connectionString))
            {
                throw new KeyNotFoundException($"No connection string found for database key '{databaseKey}'.");
            }

            var connection = new SqlConnection(connectionString);
            try
            {
                await connection.OpenAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw;
            }
            return connection;
        }
    }
}
