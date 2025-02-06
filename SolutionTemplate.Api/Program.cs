using SolutionTemplate.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Configure();

builder.Services.Configure(builder.Configuration);

var app = builder.Build();

app.ConfigureMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.ConfigureEndpoints();

app.Run();
