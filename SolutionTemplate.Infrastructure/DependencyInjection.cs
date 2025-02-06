using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolutionTemplate.Application.Abstractions.Data;
using SolutionTemplate.Application.Authentication;
using SolutionTemplate.Application.Sample;
using SolutionTemplate.Infrastructure.Authentication;
using SolutionTemplate.Infrastructure.Configuration;
using SolutionTemplate.Infrastructure.Data;
using SolutionTemplate.Infrastructure.Sample;

namespace SolutionTemplate.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseConfig>(configuration.GetRequiredSection("DatabaseConfig"));

            services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

            services.AddTransient<IAuthenticationManager, SampleAuthManager>();
            services.AddTransient<ISampleStorage, SqlSampleStorage>();

            // TODO: Register Infrastructure Here

            return services;
        }
    }
}
