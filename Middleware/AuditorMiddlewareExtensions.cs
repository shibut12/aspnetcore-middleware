using Microsoft.AspNetCore.Builder;

namespace aspnetcore_middleware.Middleware
{
    public static class AuditorMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuditor(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AuditorMiddleware>();
        }
    }
}
