using Microsoft.EntityFrameworkCore;
using Igrocom.Models;

namespace Igrocom.Data
{
    public class IgrocomContext : DbContext
    {
        public IgrocomContext (DbContextOptions<IgrocomContext> options)
            : base(options)
        {
        }

        public DbSet<Igrocom.Models.Content> Content { get; set; } = default!;
        public DbSet<Igrocom.Models.Game> Game {get; set; } = default;
        public DbSet<Igrocom.Models.User> User { get; set; } = default;

        public DbSet<Igrocom.Models.GameFiles> GameFiles {get; set; } = default;
        public DbSet<Igrocom.Models.MediaFiles> MediaFiles { get; set; } = default;

        public DbSet<UserGame> UserGame {get; set;} = default;
        public DbSet<UserContent> UserContent { get; set; } = default;

        public DbSet<Rating> Rating { get; set; } = default;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGame>().HasKey(ug => new { ug.GameId, ug.UserId });

            modelBuilder.Entity<UserGame>().HasOne(ug => ug.Game).WithMany(g => g.UserGame).HasForeignKey(ug => ug.GameId);

            modelBuilder.Entity<UserGame>().HasOne(ug => ug.User).WithMany(g => g.UserGame).HasForeignKey(ug => ug.UserId);


            modelBuilder.Entity<UserContent>().HasKey(ug => new { ug.ContentId, ug.UserId });

            modelBuilder.Entity<UserContent>().HasOne(ug => ug.Content).WithMany(g => g.UserContent).HasForeignKey(ug => ug.ContentId);

            modelBuilder.Entity<UserContent>().HasOne(ug => ug.User).WithMany(g => g.UserContent).HasForeignKey(ug => ug.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}

