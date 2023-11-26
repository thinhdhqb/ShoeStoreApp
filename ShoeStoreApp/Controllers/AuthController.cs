using Microsoft.AspNetCore.Mvc;
using ShoeStoreApp.Data;

namespace ShoeStoreApp.Controllers
{
    public class AuthController : Controller
    {
        ShoeStoreAppContext _context;
        public AuthController(ShoeStoreAppContext context) { 
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Auth/Login")]
        public async Task<IActionResult> VerifyLogin()
        {

            return View();
        }
    }
}
