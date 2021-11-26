using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialNetworkOnSharp.Models;
using SocialNetworkOnSharp.ViewsModel;

namespace SocialNetworkOnSharp.Services
{
    public class UserService
    {
        private readonly ApplicationContext applicationContext;

        public UserService(ApplicationContext applicationContext) => this.applicationContext = applicationContext;

        internal List<Participant> AllUsers() => applicationContext.Users.ToList();

        internal async Task<List<Participant>> AllUsersAsync() => await applicationContext.Users.ToListAsync();

        internal async Task<Participant> FindByIdUserAsync(int id) => await applicationContext.Users.FirstOrDefaultAsync(user => user.Id == id);

        internal async Task<Participant> FindByNickNameAsync(string nickName) => await applicationContext.Users.FirstOrDefaultAsync(user => user.NickName == nickName);

        internal Participant FindByNickName(string nickName) => applicationContext.Users.FirstOrDefault(user => user.NickName == nickName);

        internal async Task<Participant> FindByLoginAsync(string Login) => await applicationContext.Users.FirstOrDefaultAsync(user => user.Login == Login);

        internal Participant FindByLogin(string Login) => applicationContext.Users.FirstOrDefault(user => user.Login == Login);

        internal async Task<List<Participant>> ParticipantsSearchUsersList(string nickname, string principalUserNickname)
        {
            var m = FindByNickNameAsync(principalUserNickname);
            if (string.IsNullOrEmpty(nickname))
            {
                return await applicationContext.Users.AsNoTracking()
                    .Where(u => u.IsUserDeleted == false)
                    .Where(u => u.Role.Equals("user"))
                    .Where(u => !u.NickName.Equals(principalUserNickname))
                    .Where(u => !FindByNickName(principalUserNickname).FriendList.Contains(applicationContext.Friends.FirstOrDefault(f => f.NickName == u.NickName)))
                    .ToListAsync();
            }
            else
            {
                return await applicationContext.Users.AsNoTracking()
                    .Where(u => u.IsUserDeleted == false)
                    .Where(u => u.Role.Equals("user"))
                    .Where(u => !u.NickName.Equals(principalUserNickname))
                    .Where(u => u.NickName.Contains(nickname.Trim()))
                    .Where(u => !FindByNickName(principalUserNickname).FriendList.Contains(applicationContext.Friends.FirstOrDefault(f => f.NickName == u.NickName)))
                    .ToListAsync();
            }
        }

        internal FriendsDictonaryModel FriendList(string userlogin)
        {
            Participant principalUser = FindByLogin(userlogin);
            FriendsDictonaryModel friendsDictonaryModel = new();
            friendsDictonaryModel.Friends = principalUser.FriendList.Where(f => f.FriendStatus == FriendStatus.FriendList).ToList();
            friendsDictonaryModel.FriendsRequestFromMe = principalUser.FriendList.Where(f => f.FriendStatus == FriendStatus.RequestFromMe).ToList();
            friendsDictonaryModel.FriendsRequestToMe = principalUser.FriendList.Where(f => f.FriendStatus == FriendStatus.RequestToMe).ToList();
            return friendsDictonaryModel;
        }
    }
}