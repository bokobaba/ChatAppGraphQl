using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatAppGraphQl.Model {
    public class User {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string FirebaseId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime LastSeen { get; set; }

        public List<CommunitySubscription> Subscriptions { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Post> Posts { get; set; } = new List<Post>();
        public List<UserLikePost> LikedPosts { get; set; } = new List<UserLikePost>();
    }
}
