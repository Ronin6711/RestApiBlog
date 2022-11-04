using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestApiBlog.Domain;

namespace RestApiBlog.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Game> Games { get; set; }
        
        public DbSet<GameUser> GameUsers { get; set; }

        public DbSet<PublicProfile> PublicProfiles { get; set; }

        public DbSet<PostGame> PostGames { get; set; }
    }
}