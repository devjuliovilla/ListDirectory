using Microsoft.AspNetCore.Mvc;

namespace ListDirectory.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AppAuthenticationAttribute : TypeFilterAttribute
    {
        public AppAuthenticationAttribute() : base(typeof(AppAuthorizationFilter))
        {
        }
    }
}
