using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ListDirectory.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration configuration;

        public AccountController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(string password)
        {
            string storedPassword = configuration.GetValue<string>("AppSettings:Password");

            if (password == storedPassword)
                HttpContext.Session.Set("IsAuthorized", new byte[] { 1 });
                return RedirectToAction("Files", "Home");
            
            return View();
        }
    }
}
