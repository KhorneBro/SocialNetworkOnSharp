using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkOnSharp.Models
{
    public class Friend
    {
        public int Id { get; set; }
        public int? ParticipantId { get; set; }
        public Participant Participant { get; set; }
        public FriendStatus FriendStatus { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }
    }

    public enum FriendStatus : byte
    {
        FriendList = 1,
        RequestToMe = 2,
        RequestFromMe = 3
    }
}
