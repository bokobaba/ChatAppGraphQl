
using ChatAppGraphQl.DataLoaders;
using ChatAppGraphQl.Queries.CommunityQueries;
using ChatAppGraphQl.Queries.SearchQueries;

namespace ChatAppGraphQl.Queries.UserQueries {
    public class UserType : ISearchResult {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime LastSeen { get; set; }

        [IsProjected(true)]
        public IEnumerable<string> SubscriptionIds { get; set; }

        [GraphQLNonNullType]
        [UseFiltering]
        [UseSorting]
        public async Task<IEnumerable<CommunityType>> CommunitySubscriptions(
            [DataLoader] CommunityDataLoader dataLoader
        ) {
            return await dataLoader.LoadAsync(SubscriptionIds.ToArray());
        }
    }
}
