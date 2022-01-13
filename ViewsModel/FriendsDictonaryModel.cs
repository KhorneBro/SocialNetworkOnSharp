using SocialNetworkOnSharp.Models;
using System.Collections.Generic;

namespace SocialNetworkOnSharp.ViewsModel
{
    public class FriendsDictonaryModel
    {
        public IEnumerable<Friend> Friends { get; set; }
        public IEnumerable<Friend> FriendsRequestToMe { get; set; }
        public IEnumerable<Friend> FriendsRequestFromMe { get; set; }
    }
}
