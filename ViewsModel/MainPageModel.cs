using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SocialNetworkOnSharp.Models;

namespace SocialNetworkOnSharp.ViewsModel
{
    public class MainPageModel
    {
        public MainPageModel(User user)
        {
            User = user;
        }

        public User User { get; set; }
        public IFormFile AddAvatarpicture { get; set; }
        public AddInfoUserModel addInfo { get; set; }
    }
}
