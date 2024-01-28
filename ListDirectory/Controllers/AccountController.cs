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
            {
                HttpContext.Session.SetInt32("IsAuthorized", 1);
                return RedirectToAction("Files", "Files");
            }
            
            return View();
        }
    }
}
