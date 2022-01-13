using System.Collections.Generic;

namespace SocialNetworkOnSharp.Models
{
    public class TheCreature
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Initiative { get; set; }
        public bool IsHero { get; set; }
        public string MadeBy { get; set; }
        public List<Room> Room { get; set; } = new List<Room>();
    }
}
