using ChatAppGraphQl.Model;
using Microsoft.EntityFrameworkCore;

namespace ChatAppGraphQl.Data {
    public class ApplicationDbContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<User>()
                .HasIndex(u => u.Username).IsUnique();

            builder.Entity<User>()
                .HasMany(e => e.Messages)
                .WithOne(m => m.User)
                .HasPrincipalKey(u => u.Username)
                .HasForeignKey(m => m.Username);
        }
    }
}
