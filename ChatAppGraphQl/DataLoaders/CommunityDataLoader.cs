
using ChatAppGraphQl.Queries.CommunityQueries;
using ChatAppGraphQl.Services.CommunityRepository;

namespace ChatAppGraphQl.DataLoaders {
    public class CommunityDataLoader : BatchDataLoader<string, CommunityType> {
        private readonly ICommunityRepository _repository;

        public CommunityDataLoader(
            ICommunityRepository repository,
            IBatchScheduler batchScheduler,
            DataLoaderOptions options
        ) : base(batchScheduler, options) {
            _repository = repository;
        }

        protected override async Task<IReadOnlyDictionary<string, CommunityType>> LoadBatchAsync(
            IReadOnlyList<string> keys, CancellationToken cancellationToken) {
            IEnumerable<CommunityType> communities = await _repository.GetByIds(keys);

            return communities.ToDictionary(u => u.Name);
        }
    }
}