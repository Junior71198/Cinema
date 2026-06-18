using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cinema.Areas.Identity.Controllers
{
    [Area(CD.IDENTITY_AREA)]
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            var user = new ApplicationUser()
            {
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Email = registerVM.Email,
                UserName = registerVM.UserName
            };
            var result = await _userManager.CreateAsync(user, registerVM.Password);
            if (!result.Succeeded)
            {

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(registerVM);
            }
            // register
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { CD.IDENTITY_AREA, userId = user.Id, token = token }, Request.Scheme);
            await _emailSender.SendEmailAsync(registerVM.Email,
                  "EmailConfirmation",
                  $"<h1>Please click <a href='{link}'>here</a> to confirm your account</h1>");
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult ResendConfirmation()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResendConfirmation(ResendConfirmationVM resendConfirmationVM)
        {
            var user = await _userManager.FindByNameAsync(resendConfirmationVM.UserNameOrEmail) ??
                      await _userManager.FindByEmailAsync(resendConfirmationVM.UserNameOrEmail);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { CD.IDENTITY_AREA, userId = user.Id, token = token }, Request.Scheme);
            await _emailSender.SendEmailAsync(
                user.Email,
                  "EmailConfirmation",
                  $"<h1>Please click <a href='{link}'>here</a> to confirm your account</h1>");
            return RedirectToAction(nameof(Login));


            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid UserName or Password.");
                return View(loginVM);
            }
            var user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail) ??
                       await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Your account is locked out.");

                }
                else if (!result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, "Please confirm your email to login.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid UserName or Password.");

                }
                return View(loginVM);
            }
            return RedirectToAction("index", "Home", new { area = CD.CUSTOMER_AREA });
        }
    }
}

