using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkOnSharp.Models
{
    [Table("Participant")]
    public class Participant
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }
        public string MyHistory { get; set; }
        public bool UseMyHistory { get; set; }
        public bool UserAddInfo { get; set; }
        public bool IsUserDeleted { get; set; }
        public List<Room> Rooms { get; set; } = new List<Room>();
        public ICollection<Friend> FriendList { get; set; } = new List<Friend>();
    }
}
