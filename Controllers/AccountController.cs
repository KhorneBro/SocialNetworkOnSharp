using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetworkOnSharp.Models;
using SocialNetworkOnSharp.ViewsModel;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialNetworkOnSharp.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext applicationContext;

        public AccountController(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        [HttpGet]
        public IActionResult Register() { return View(); }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                Participant user = await applicationContext.Users.FirstOrDefaultAsync(u => u.Login == registerModel.Login);
                if (user == null)
                {
                    user = new Participant
                    {
                        Login = registerModel.Login.Trim(),
                        Password = registerModel.Password.Trim(),
                        Role = "user",
                        NickName = registerModel.NickName.Trim()
                    };
                    applicationContext.Users.Add(user);
                    await applicationContext.SaveChangesAsync();
                    await Authenticate(user);

                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", "Такой пользователь уже есть");
                }
                return View(registerModel);
            }
            else
            {
                return View(registerModel);
            }
        }

        [HttpGet]
        public IActionResult Login() { return View(); }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                Participant user = await applicationContext.Users.FirstOrDefaultAsync(u => u.Login == loginModel.Login && u.Password == loginModel.Password);
                if (user != null)
                {
                    await Authenticate(user);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(loginModel);
        }

        private async Task Authenticate(Participant user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
