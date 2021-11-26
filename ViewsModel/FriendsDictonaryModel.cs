using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialNetworkOnSharp.Models;

namespace SocialNetworkOnSharp.ViewsModel
{
    public class FriendsDictonaryModel
    {
        public IEnumerable<Friend> Friends { get; set; }
        public IEnumerable<Friend> FriendsRequestToMe { get; set; }
        public IEnumerable<Friend> FriendsRequestFromMe { get; set; }
    }
}
