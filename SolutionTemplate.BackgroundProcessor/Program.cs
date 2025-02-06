using MassTransit;
using SolutionTemplate.Application;
using SolutionTemplate.BackgroundProcessor.Consumers;
using SolutionTemplate.Infrastructure;
using SolutionTemplate.Infrastructure.Configuration;

var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostContext, config) =>
    {
        config.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddApplication();
        services.AddInfrastructure(hostContext.Configuration);        

        var queueConfig = hostContext.Configuration.GetSection("QueueConfig").Get<QueueConfig>() ?? throw new ConfigurationException("No queue config present.");

        services.AddMassTransit(mt =>
        {
            mt.AddConsumer<SampleHappenedConsumer>();            

            // This configuration will try 3 times, 5s apart. If all fail, it will wait 5, then 15, then 30 minutes. This will result in up to 9 attempts
            mt.AddConfigureEndpointsCallback((context, name, cfg) =>
            {
                cfg.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30)));
                cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
            });

            mt.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(queueConfig.Host, queueConfig.Port, "/", h =>
                {
                    h.Username(queueConfig.Username);
                    h.Password(queueConfig.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    });

IConfiguration configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddJsonFile("appsettings.local.json", true)
        .Build();

//var isWindows = bool.Parse(configuration["EnvironmentSettings:UseWindowsService"] ?? "false");

//if (isWindows)
//{
//    hostBuilder.UseWindowsService(cfg =>
//    {
//        cfg.ServiceName = "OnePoint ODS Queue Consumer Service";
//    });
//}

var host = hostBuilder.Build();
await host.RunAsync();

