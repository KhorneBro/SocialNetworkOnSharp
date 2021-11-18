using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialNetworkOnSharp.Models;

namespace SocialNetworkOnSharp.Services
{
    public class UserService
    {
        private ApplicationContext applicationContext;

        public UserService(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        internal List<Participant> AllUsers() => applicationContext.Users.ToList();
        internal async Task<List<Participant>> AllUsersAsync() => await applicationContext.Users.ToListAsync();
        internal async Task<Participant> FindByIdUserAsync(int id) => await applicationContext.Users.FirstOrDefaultAsync(user => user.Id == id);
        internal async Task<Participant> FindByNickNameAsync(string nickName) => await applicationContext.Users.FirstOrDefaultAsync(user => user.NickName == nickName);
        internal Participant FindByNickName(string nickName) => applicationContext.Users.FirstOrDefault(user => user.NickName == nickName);
        internal async Task<Participant> FindByLoginAsync(string Login) => await applicationContext.Users.FirstOrDefaultAsync(user => user.Login == Login);
        internal Participant FindByLogin(string Login) => applicationContext.Users.FirstOrDefault(user => user.Login == Login);
        internal List<Participant> ParticipantsSearchFriendList(string nickname, string principalUserNickname)
        {
            if (string.IsNullOrEmpty(nickname))
            {
                return applicationContext.Users.AsNoTracking()
                    .ToList()
                    .Where(u => u.Role.Equals("user", StringComparison.Ordinal))
                    .Where(u => !u.NickName.Equals(principalUserNickname, StringComparison.Ordinal))
                    .Where(u => !IsThisUserInMyFriendListByUsername(principalUserNickname, u.NickName))
                    .ToList();
            }
            else
            {
                return applicationContext.Users.AsNoTracking()
                    .ToList()
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
                  .FriendList
                  .FirstOrDefault(f => f.FriendsId == FindByNickName(friendUsername).Id) is not null;
        }
        internal Dictionary<string, List<Participant>> FriendList(string userlogin)
        {
            Participant principalUser = FindByLogin(userlogin);
            Dictionary<string, List<Participant>> friendListsToReturn = new();

            List<Participant> allUsers = applicationContext.Users.AsNoTracking().ToList();

            var test = allUsers.Where(u => principalUser.FriendList.Contains(applicationContext.Friends.First(f => f.FriendsId == u.Id))).ToList();

            friendListsToReturn.Add("FriendList",
                allUsers.Join(
                principalUser.FriendList,
                u => u.Id,
                f => f.FriendsId,
                (u, f) => new Participant { Id = f.FriendsId, Avatar = u.Avatar, NickName = u.NickName })
                .ToList());

            friendListsToReturn.Add("RequestToMe", allUsers.Join(
                principalUser.FriendRequestToMe,
                u => u.Id,
                f => f.FriendsId,
                (u, f) => new Participant { Id = f.FriendsId, Avatar = u.Avatar, NickName = u.NickName })
                .ToList());

            friendListsToReturn.Add("RequestFromMe", allUsers.Join(
                principalUser.FriendRequestFromMe,
                u => u.Id,
                f => f.FriendsId,
                (u, f) => new Participant { Id = f.FriendsId, Avatar = u.Avatar, NickName = u.NickName })
                .ToList());
            return friendListsToReturn;
        }
    }
}