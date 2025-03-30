using Microsoft.AspNetCore.Builder;

namespace GameOfLife.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<Middleware.ExceptionHandlingMiddleware>();
        }
    }
}