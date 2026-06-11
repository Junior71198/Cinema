using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cinema.Areas.Identity.Controllers
{
        [Area(CD.IDENTITY_AREA)]
    public class AcountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public AcountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
