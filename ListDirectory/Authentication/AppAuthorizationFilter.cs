using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace ListDirectory.Authentication
{
    public class AppAuthorizationFilter : IAuthorizationFilter
    {
        private readonly IConfiguration configuration;

        public AppAuthorizationFilter(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool enableSecurity = configuration.GetValue<bool>("AppSettings:EnableSecurity");
            if (!enableSecurity)
                return;

            if (context.HttpContext.Session.GetInt32("IsAuthorized") != 1)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }
}
