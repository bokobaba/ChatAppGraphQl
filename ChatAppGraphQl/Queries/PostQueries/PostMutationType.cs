using ChatAppGraphQl.DataLoaders;
using ChatAppGraphQl.Queries.CommunityQueries;

namespace ChatAppGraphQl.Queries.PostQueries {
    public class PostMutationType {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public string CommunityName { get; set; }
        public string CreatorId { get; set; }
    }
}
