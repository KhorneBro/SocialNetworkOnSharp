﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialNetworkOnSharp.Models;
using SocialNetworkOnSharp.Services;
using SocialNetworkOnSharp.ViewsModel;

namespace SocialNetworkOnSharp.Controllers
{
    [Authorize(Roles = "user")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService userService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ApplicationContext applicationContext;
        public HomeController(
               ILogger<HomeController> logger,
               UserService userService,
               IWebHostEnvironment webHostEnvironment,
               ApplicationContext applicationContext)
        {
            _logger = logger;
            this.userService = userService;
            this.webHostEnvironment = webHostEnvironment;
            this.applicationContext = applicationContext;

        }

        public ActionResult Index(int? id)
        {
            MainPageModel pageModel = new MainPageModel();
            if (id == null)
            {
                pageModel.User = userService.FindByLogin(User.Identity.Name.ToString());
            }
            else
            {
                pageModel.User = applicationContext.Users.FirstOrDefault(u => u.Id == id);
            }

            return View(pageModel);
        }

        [HttpPost]
        public async Task<ActionResult> AddAvatar(MainPageModel mainPageModel)
        {
            string[] permittedExtensions = { ".jpg", ".png", ".jpeg" };


            if (mainPageModel.AddAvatarpicture is null)
            {
                ModelState.AddModelError("AddAvatarpicture", "Файл не передан");
                return RedirectToAction("Index");
            }
            else if (mainPageModel.AddAvatarpicture.Length > 31457280)
            {
                ModelState.AddModelError("AddAvatarpicture", "Файл не должен превышать размер 50Мб");
                return RedirectToAction("Index");
            }
            else if (string.IsNullOrEmpty(Path.GetExtension(mainPageModel.AddAvatarpicture.FileName).ToLowerInvariant()) ||
                !permittedExtensions.Contains(Path.GetExtension(mainPageModel.AddAvatarpicture.FileName).ToLowerInvariant()))
            {
                ModelState.AddModelError("AddAvatarpicture", "Не верный формат файла");
                return RedirectToAction("Index");
            }
            else
            {
                Participant user = applicationContext.Users.Find(mainPageModel.User.Id);
                if (!string.IsNullOrEmpty(user.Avatar))
                {
                    FileInfo fileInfo = new FileInfo(webHostEnvironment.WebRootPath + user.Avatar);
                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }
                    else
                    {
                        ModelState.AddModelError("AddAvatarpicture", "Файл принадлежит пользователю, но удален из хранилища");
                    }
                    user.Avatar = "";
                }
                string path = "/Avatars/" + Path.GetRandomFileName() + Path.GetExtension(mainPageModel.AddAvatarpicture.FileName).ToLowerInvariant();
                using (var filestream = new FileStream(webHostEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await mainPageModel.AddAvatarpicture.CopyToAsync(filestream);
                }

                user.Avatar = path;
                await applicationContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult FindFriend(string nickname)
        {
            return View("FindFriend", userService.ParticipantsSearchFriendList(nickname, userService.FindByLogin(User.Identity.Name.ToString()).NickName));
        }

        [HttpGet]
        public ActionResult FriendList()
        {
            Dictionary<string, List<Participant>> threeTypesFriendlist = userService.FriendList(User.Identity.Name.ToString());
            ViewBag.FriendList = threeTypesFriendlist["FriendList"];
            ViewBag.RequestToMe = threeTypesFriendlist["RequestToMe"];
            ViewBag.RequestFromMe = threeTypesFriendlist["RequestFromMe"];
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
