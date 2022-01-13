﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkOnSharp.Models;

namespace SocialNetworkOnSharp.ViewsModel
{
    public class MainPageModel
    {
        public Participant User { get; set; }
        [BindProperty]
        public IFormFile AddAvatarpicture { get; set; }
        [BindProperty]
        public AddInfoUserModel addInfo { get; set; }
    }
}
