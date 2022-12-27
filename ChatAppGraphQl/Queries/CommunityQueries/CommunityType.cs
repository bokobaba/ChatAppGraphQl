using ChatAppGraphQl.Queries.UserQueries;
using ChatAppGraphQl.DataLoaders;
using ChatAppGraphQl.Queries.PostQueries;

namespace ChatAppGraphQl.Queries.CommunityQueries {
    public class CommunityType {
        public string Name { get; set; } = string.Empty;
        public string? BannerImage { get; set; }

        [IsProjected(true)]
        public IEnumerable<string> SubscriberIds { get; set; }

        [UseFiltering]
        [UseSorting]
        public async Task<IEnumerable<UserType>> Subscribers([DataLoader] UserDataLoader dataLoader) {
            return await dataLoader.LoadAsync(SubscriberIds.ToArray());
        }

        [IsProjected(true)]
        public IEnumerable<Guid> PostIds { get; set; }

        [UseFiltering]
        [UseSorting]
        public async Task<IEnumerable<PostType>> Posts([DataLoader] PostDataLoader dataLoader) {
            return await dataLoader.LoadAsync(PostIds.ToArray());
        }
    }
}
