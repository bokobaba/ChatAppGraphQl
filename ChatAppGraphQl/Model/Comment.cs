using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatAppGraphQl.Model {
    public class Comment {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }

        public string CreatorId { get; set; }
        public User Creator { get; set; }

        public Guid PostId { get; set; }
        public Post Post { get; set; }

        public Guid? ReplyToId { get; set; }
        public Comment? ReplyTo { get; set; }

        public List<Comment> Replies { get; set; } = new List<Comment>();
    }
}
