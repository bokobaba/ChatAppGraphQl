using ChatAppGraphQl.Queries.CommentQueries;
using ChatAppGraphQl.Queries.UserQueries;
using ChatAppGraphQl.Services.CommentRepository;

namespace ChatAppGraphQl.DataLoaders {
    public class CommentDataLoader : BatchDataLoader<Guid, CommentType> {
        private readonly ICommentRepository _repository;

        public CommentDataLoader(ICommentRepository repository, IBatchScheduler batchScheduler, 
            DataLoaderOptions options) : base(batchScheduler, options) {
            _repository = repository;
        }

        protected override async Task<IReadOnlyDictionary<Guid, CommentType>> LoadBatchAsync(
            IReadOnlyList<Guid> keys, CancellationToken cancellationToken) {
            IEnumerable<CommentType> messages = await _repository.GetByIds(keys);
            return messages.ToDictionary(m => m.Id);
        }
    }
}
