namespace ListDirectory.Middleware
{
    public class SecurityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public SecurityMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            bool enableSecurity = _configuration.GetValue<bool>("AppSettings:EnableSecurity");

            if (enableSecurity && !context.User.Identity.IsAuthenticated)
            {
                context.Response.Redirect("/Account/Login");
                return;
            }

            await _next(context);
        }
    }
}