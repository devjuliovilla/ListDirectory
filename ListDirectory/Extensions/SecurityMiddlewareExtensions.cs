using ListDirectory.Middleware;

namespace ListDirectory.Extensions
{
    public static class SecurityMiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SecurityMiddleware>();
        }
    }
}