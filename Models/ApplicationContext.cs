using Microsoft.EntityFrameworkCore;

namespace SocialNetworkOnSharp.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Participant>()
                .Property(u => u.Avatar)
                .HasDefaultValue("/Avatars/avatar_alt.png");
        }

        public DbSet<Participant> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<TheCreature> TheCreatures { get; set; }
        public DbSet<Friend> Friends { get; set; }
    }
}
