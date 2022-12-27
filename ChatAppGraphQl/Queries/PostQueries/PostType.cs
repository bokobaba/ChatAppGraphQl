using ChatAppGraphQl.DataLoaders;
using ChatAppGraphQl.Queries.CommentQueries;
using ChatAppGraphQl.Queries.CommunityQueries;
using ChatAppGraphQl.Queries.UserQueries;

namespace ChatAppGraphQl.Queries.PostQueries {
    public class PostType {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }

        [IsProjected(true)]
        public string CommunityName { get; set; }
        public async Task<CommunityType> Community([DataLoader] CommunityDataLoader dataLoader) {
            return await dataLoader.LoadAsync(CommunityName, CancellationToken.None);
        }

        [IsProjected(true)]
        public string CreatorId { get; set; }
        public async Task<UserType> Creator([DataLoader] UserDataLoader dataLoader) {
            return await dataLoader.LoadAsync(CreatorId, CancellationToken.None);
        }

        [IsProjected(true)]
        public IEnumerable<Guid> CommentIds { get; set; }
        [UseFiltering]
        [UseSorting]
        public async Task<IEnumerable<CommentType>> Comments([DataLoader] CommentDataLoader dataLoader) {
            return await dataLoader.LoadAsync(CommentIds.ToArray(), CancellationToken.None);
        }

        [IsProjected(true)]
        public IEnumerable<string> LikeIds { get; set; }

        [UseFiltering]
        [UseSorting]
        public async Task<IEnumerable<UserType>> UserLikes([DataLoader] UserDataLoader dataLoader) {
            return await dataLoader.LoadAsync(LikeIds.ToArray(), CancellationToken.None);
        }
    }
}
