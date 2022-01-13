using System.Collections.Generic;

namespace SocialNetworkOnSharp.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Owner { get; set; }
        public string RoomName { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public List<Participant> Users { get; set; } = new List<Participant>();
        public List<TheCreature> TheCreatures { get; set; } = new List<TheCreature>();
    }
}
