using Dapper;
using SolutionTemplate.Application.Abstractions.Data;
using SolutionTemplate.Application.Sample;
using SolutionTemplate.Domain.Sample;

namespace SolutionTemplate.Infrastructure.Sample
{
    internal class SqlSampleStorage : ISampleStorage
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public SqlSampleStorage(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<List<Item>> GetValues(CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT [Key], [Value] FROM [dbo].[ExampleTable]";

            var results = new List<SampleEntity>();
            using(var connection = await _connectionFactory.CreateConnection(Databases.Default, cancellationToken))
            {
                results = (await connection.QueryAsync<SampleEntity>(sql)).ToList();
            }

            return results.Select(r => new Item(r.Key, r.Value)).ToList();
        }
    }
}
