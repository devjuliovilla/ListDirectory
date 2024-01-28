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
            bool enableAuthentication = configuration.GetValue<bool>("AppSettings:EnableAuthentication");

            if (!enableAuthentication)
            {
                return;
            }
        }
    }
}
