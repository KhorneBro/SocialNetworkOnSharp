using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SocialNetworkOnSharp.ViewsModel;
using SocialNetworkOnSharp.Services;

namespace SocialNetworkOnSharp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AjaxActionsController : ControllerBase
    {
        private UserService userService { get; set; }

        public AjaxActionsController(UserService userService)
        {
            this.userService = userService;
        }

        //[Route("/changeAvatar/{id}")]
        [HttpGet]
        public JsonResult ChangeAvatar(AddInfoUserModel addInfoUserModel, int id)
        {
            bool success = true;
            //await userService.AddInfoToUserAsync(addInfoUserModel);
            if (id is not 0)
            {
                success = true;
            }
            return success ? new JsonResult($"Id = {id}") : new JsonResult($"Id не найдено");
        }
    }
}
