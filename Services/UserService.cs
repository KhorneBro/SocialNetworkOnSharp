using SocialNetworkOnSharp.Models;
using SocialNetworkOnSharp.ViewsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SocialNetworkOnSharp.Services
{
    public class UserService
    {
        private ApplicationContext applicationContext;

        public UserService(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        internal List<User> Users() => applicationContext.Users.ToList();
        internal async Task<User> FindByidUserAsync(int id) => await Task.Run(() => applicationContext.Users.First(user => user.Id == id));
        internal async Task<User> FindByidNickNameAsync(string nickName) => await Task.Run(() => applicationContext.Users.First(user => user.NickName == nickName));
        //internal async Task AddInfoToUserAsync(AddInfoUserModel addInfoUserModel)
        //{

        //}

        internal bool IsThisUserInMyFriendList()
        {
            return true;
        }

        internal void asda(int idUser, int idFriend)
        {
            User user = applicationContext.Users.Find(idUser);
            User friend = applicationContext.Users.Find(idFriend);
            user.FriendList.Add(new Friend { FriendsId = friend.Id });
            friend.FriendList.Add(new Friend { FriendsId = user.Id });

            user.FriendList.Remove(applicationContext.Friends.First(x => x.FriendsId == friend.Id));
        }
    }
}
