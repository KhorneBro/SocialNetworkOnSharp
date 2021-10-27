using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SocialNetworkOnSharp.Models;
using SocialNetworkOnSharp.Services;
using SocialNetworkOnSharp.ViewsModel;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace SocialNetworkOnSharp.Controllers
{
    [Authorize(Roles = "user")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserService userService;
        private IWebHostEnvironment webHostEnvironment;
        private ApplicationContext applicationContext;




        public HomeController(ILogger<HomeController> logger,
               UserService userService,
               IWebHostEnvironment webHostEnvironment,
               ApplicationContext applicationContext)
        {
            _logger = logger;
            this.userService = userService;
            this.webHostEnvironment = webHostEnvironment;
            this.applicationContext = applicationContext;

        }

        public IActionResult Index()
        {
            User user = userService.FindByidNickName(HttpContext.User.Identity.Name.ToString());
            MainPageModel pageModel = new MainPageModel(user);

            return View(pageModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAvatar([FormBody] MainPageModel mainPageModel)
        {
            var ext = Path.GetExtension(mainPageModel.AddAvatarpicture.FileName).ToLowerInvariant();
            string[] permittedExtensions = { ".jpg", ".png", ".jpeg" };


            if (mainPageModel.AddAvatarpicture == null)
            {
                ModelState.AddModelError("", "Файл не передан");
                return RedirectToAction("Index");
            }
            else if (mainPageModel.AddAvatarpicture.Length > 31457280)
            {
                ModelState.AddModelError("", "Файл не должен превышать размер 50Мб");
                return RedirectToAction("Index");
            }
            else if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                ModelState.AddModelError("", "Не верный формат файла");
                return RedirectToAction("Index");
            }
            else
            {
                User user = applicationContext.Users.Find(mainPageModel.User.Id);
                if (!string.IsNullOrEmpty(user.Avatar))
                {
                    FileInfo fileInfo = new FileInfo(webHostEnvironment.WebRootPath + user.Avatar);
                    if (fileInfo.Exists)
                        fileInfo.Delete();
                    else
                        ModelState.AddModelError("", "Файл принадлежит пользователю, но удален из хранилища");
                    user.Avatar = "";
                    await applicationContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                string path = "/Avatars/" + Path.GetRandomFileName() + ext;
                using (var filestream = new FileStream(webHostEnvironment.WebRootPath + path, FileMode.Create))
                    await mainPageModel.AddAvatarpicture.CopyToAsync(filestream);
                user.Avatar = path;
                await applicationContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
