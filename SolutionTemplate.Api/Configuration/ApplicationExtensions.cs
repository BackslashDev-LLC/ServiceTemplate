using Microsoft.AspNetCore.HttpOverrides;

namespace SolutionTemplate.Api.Configuration
{
    internal static class ApplicationExtensions
    {
        public static void ConfigureMiddleware(this IApplicationBuilder app)
        {            
            app.ApplySwaggerMiddleware();

            app.Use(async (ctx, next) =>
            {
                ctx.Request.EnableBuffering();
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto });
        }

        public static void ConfigureEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapControllers();
        }
    }
}
