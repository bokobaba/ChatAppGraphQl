using ChatAppGraphQl.Queries.UserQueries;
using ChatAppGraphQl.Model;
using ChatAppGraphQl.Services.UserRepository;
using FirebaseAdmin;
using FirebaseAdmin.Auth;

namespace ChatAppGraphQl.DataLoaders {
    public class UserDataLoader : BatchDataLoader<string, UserType> {
        private readonly IUserRepository _repository;

        public UserDataLoader(
            IUserRepository repository, 
            IBatchScheduler batchScheduler,
            DataLoaderOptions options
        ): base(batchScheduler, options) {
            _repository = repository;
        }

        protected override async Task<IReadOnlyDictionary<string, UserType>> LoadBatchAsync(
            IReadOnlyList<string> keys, CancellationToken cancellationToken) {
            IEnumerable<UserType> users = await _repository.GetByIds(keys);

            return users.ToDictionary(u => u.Id);
        }
    }
}
