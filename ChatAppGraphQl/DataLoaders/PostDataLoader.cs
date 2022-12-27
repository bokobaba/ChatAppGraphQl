
using ChatAppGraphQl.Queries.PostQueries;
using ChatAppGraphQl.Services.PostRepository;

namespace ChatAppGraphQl.DataLoaders {
    public class PostDataLoader : BatchDataLoader<Guid, PostType> {
        private readonly IPostRepository _repository;

        public PostDataLoader(IPostRepository repository, IBatchScheduler batchScheduler,
            DataLoaderOptions options) : base(batchScheduler, options) {
            _repository = repository;
        }

        protected override async Task<IReadOnlyDictionary<Guid, PostType>> LoadBatchAsync(
            IReadOnlyList<Guid> keys, CancellationToken cancellationToken) {
            IEnumerable<PostType> posts = await _repository.GetByIds(keys);
            return posts.ToDictionary(m => m.Id);
        }
    }
}
