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

        internal List<Participant> Users() => applicationContext.Users.ToList();
        internal async Task<Participant> FindByIdUserAsync(int id) => await Task.Run(() => applicationContext.Users.FirstOrDefault(user => user.Id == id));
        internal async Task<Participant> FindByNickNameAsync(string nickName) => await Task.Run(() => applicationContext.Users.FirstOrDefault(user => user.NickName == nickName));
        internal Participant FindByNickName(string nickName) => applicationContext.Users.FirstOrDefault(user => user.NickName == nickName);
        internal async Task<Participant> FindByLoginAsync(string Login) => await Task.Run(() => applicationContext.Users.FirstOrDefault(user => user.Login == Login));
        internal Participant FindByLogin(string Login) => applicationContext.Users.FirstOrDefault(user => user.Login == Login);      

        internal List<Participant> ParticipantsSearchFriendList(string nickname, string principalUserNickname)
        {
            if (string.IsNullOrEmpty(nickname))
            {
               return Users()
                     .Where(u => u.Role.Equals("user", StringComparison.Ordinal))
                     .Where(u => !u.NickName.Equals(principalUserNickname, StringComparison.Ordinal))
                     .Where(u => !IsThisUserInMyFriendListByUsername(principalUserNickname, u.NickName))
                     .ToList();
            }
            else
            {
               return Users()
                    .Where(u => u.Role.Equals("user", StringComparison.Ordinal))
                    .Where(u => !u.NickName.Equals(principalUserNickname, StringComparison.Ordinal))
                    .Where(u => u.NickName.Contains(nickname.Trim()))
                    .Where(u => !IsThisUserInMyFriendListByUsername(principalUserNickname, u.NickName))
                    .ToList();
            }
        }

        internal bool IsThisUserInMyFriendListByUsername(string principalUser, string friendUsername)
        {
            return FindByNickName(principalUser)
                  .FriendList?
                  .FirstOrDefault(f => f.FriendsId == FindByNickName(friendUsername).Id) is not null;
        }

        //internal async void makeRequestForAddingFriend(int id) {
        //    User friend = await applicationContext.Users.FindAsync(id);
        //    User owner = await FindByidNickNameAsync
        //}

        internal void asda(int idUser, int idFriend)
        {
            Participant user = applicationContext.Users.Find(idUser);
            Participant friend = applicationContext.Users.Find(idFriend);
            user.FriendList.Add(new Friend { FriendsId = friend.Id });
            friend.FriendList.Add(new Friend { FriendsId = user.Id });

            user.FriendList.Remove(applicationContext.Friends.First(x => x.FriendsId == friend.Id));
        }
    }
}
