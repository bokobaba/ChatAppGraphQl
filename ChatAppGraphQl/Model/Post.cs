using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatAppGraphQl.Model {
    public class Post {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }

        public string CreatorId { get; set; }
        public User Creator { get; set; }

        public string CommunityName { get; set; }
        public Community Community { get; set; }

        public List<UserLikePost> Likes { get; set; } = new List<UserLikePost>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
