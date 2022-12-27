
using ChatAppGraphQl.DataLoaders;
using ChatAppGraphQl.Queries.UserQueries;
using ChatAppGraphQl.Queries.SearchQueries;
using ChatAppGraphQl.Queries.PostQueries;

namespace ChatAppGraphQl.Queries.CommentQueries {
    public class CommentType : ISearchResult {
        [GraphQLNonNullType]
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }

        [IsProjected(true)]
        public string CreatorId { get; set; }

        public async Task<UserType> User([DataLoader] UserDataLoader dataLoader) {
            return await dataLoader.LoadAsync(CreatorId, CancellationToken.None);
        }

        [IsProjected(true)]
        public Guid PostId { get; set; }
        public async Task<PostType> Post([DataLoader] PostDataLoader dataLoader) {
            return await dataLoader.LoadAsync(PostId, CancellationToken.None);
        }

        [IsProjected(true)]
        public Guid? ReplyToId { get; set; }
        public async Task<CommentType?> ReplyTo([DataLoader] CommentDataLoader dataLoader) {
            if (ReplyToId == null)
                return null;
            else
                return (CommentType?)await dataLoader.LoadAsync(ReplyToId, CancellationToken.None);
        }

        [IsProjected(true)]
        public IEnumerable<Guid> ReplyIds { get; set; }

        [UseFiltering]
        [UseSorting]
        public async Task<IEnumerable<CommentType>> Replies([DataLoader] CommentDataLoader dataLoader) {
            return await dataLoader.LoadAsync(ReplyIds.ToArray(), CancellationToken.None);
        }
    }
}