using ChatAppGraphQl.Model;
using Microsoft.EntityFrameworkCore;

namespace ChatAppGraphQl.Data {
    public class ApplicationDbContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<CommunitySubscription> CommunitySubscriptions { get; set; }
        public DbSet<UserLikePost> UserLikePosts { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<User>()
                .HasIndex(u => u.Username).IsUnique();

            builder.Entity<Community>()
                .HasIndex(c => c.Name).IsUnique();

            builder.Entity<CommunitySubscription>()
                .HasKey(cs => new { cs.SubscriberId, cs.CommunityName });
            builder.Entity<CommunitySubscription>()
                .HasOne(cs => cs.Subscriber)
                .WithMany(s => s.Subscriptions)
                .HasPrincipalKey(s => s.FirebaseId)
                .HasForeignKey(cs => cs.SubscriberId);
            builder.Entity<CommunitySubscription>()
                .HasOne(cs => cs.Community)
                .WithMany(c => c.Subscribers)
                .HasPrincipalKey(c => c.Name)
                .HasForeignKey(cs => cs.CommunityName);

            builder.Entity<User>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.Creator)
                .HasPrincipalKey(u => u.FirebaseId)
                .HasForeignKey(c => c.CreatorId);

            builder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.Creator)
                .HasPrincipalKey(u => u.FirebaseId)
                .HasForeignKey(p => p.CreatorId);

            builder.Entity<Community>()
                .HasMany(c => c.Posts)
                .WithOne(p => p.Community)
                .HasPrincipalKey(c => c.Name)
                .HasForeignKey(p => p.CommunityName)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserLikePost>()
                .HasKey(ulp => new { ulp.UserId, ulp.PostId });
            builder.Entity<UserLikePost>()
                .HasOne(ulp => ulp.User)
                .WithMany(u => u.LikedPosts)
                .HasPrincipalKey(u => u.FirebaseId)
                .HasForeignKey(ulp => ulp.UserId);
            builder.Entity<UserLikePost>()
                .HasOne(ulp => ulp.Post)
                .WithMany(p => p.Likes)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(ulp => ulp.PostId);

            builder.Entity<Comment>()
                .HasMany(c => c.Replies)
                .WithOne(r => r.ReplyTo)
                .HasPrincipalKey(c => c.Id)
                .HasForeignKey(r => r.ReplyToId);
        }
    }
}
