using Asp.Versioning;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SolutionTemplate.Api.Providers;
using SolutionTemplate.Application;
using SolutionTemplate.Infrastructure;
using SolutionTemplate.Infrastructure.Configuration;
using System.Text;

namespace SolutionTemplate.Api.Configuration
{
    internal static class ApplicationBuilderExtensions
    {
        public static void Configure(this ConfigurationManager configuration)
        {
            configuration.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
        }

        public static void Configure(this IServiceCollection services, IConfiguration configuration)
        {
            services
               .AddControllers()
               .ConfigureApiBehaviorOptions(options =>
               {
                   options.InvalidModelStateResponseFactory = context =>
                   {
                       return new BadRequestObjectResult(context.ModelState);
                   };
               });

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ApiVersionReader = new HeaderApiVersionReader();
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            var queueConfig = configuration.GetSection("QueueConfig").Get<QueueConfig>();

            if (queueConfig == null) throw new ConfigurationException("No queue configuration");

            services.AddMassTransit(mt =>
            {
                mt.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(queueConfig.Host, queueConfig.Port, "/", h =>
                    {
                        h.Username(queueConfig.Username);
                        h.Password(queueConfig.Password);
                    });
                });
            });

            var appSettingsSection = configuration.GetRequiredSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings!.JwtClientSecret)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = appSettings.JwtAudience,
                    ValidIssuer = appSettings.JwtIssuer
                };
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["jwt"];
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddSwagger();

            services.AddTransient<IAuthenticationProvider, AuthenticationProvider>();

            services.AddApplication();
            services.AddInfrastructure(configuration);
        }
    }
}
