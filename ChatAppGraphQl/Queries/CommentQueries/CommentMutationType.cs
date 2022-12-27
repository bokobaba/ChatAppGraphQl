using ChatAppGraphQl.DataLoaders;
using ChatAppGraphQl.Queries.PostQueries;
using ChatAppGraphQl.Queries.UserQueries;

namespace ChatAppGraphQl.Queries.CommentQueries {
    public class CommentMutationType {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }
        public string CreatorId { get; set; }
        public Guid PostId { get; set; }
        public Guid? ReplyToId { get; set; }
    }
}
