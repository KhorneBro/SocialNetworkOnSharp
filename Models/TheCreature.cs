using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
