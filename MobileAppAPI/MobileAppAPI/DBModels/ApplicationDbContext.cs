using Microsoft.EntityFrameworkCore;
using MobileAppAPI.DBModels.Accounts;
using MobileAppAPI.DBModels.Content;

namespace MobileAppAPI.DBModels
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<PostModel> Posts { get; set; }
        public DbSet<RefreshTokenModel> RefreshTokens { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}