using ChatAppGraphQl.Queries.UserQueries;

namespace ChatAppGraphQl.Services.UserRepository {
    public interface IUserRepository {
        public Task<UserMutationType> Create(string firebaseId, UserInput request);
        public Task<UserMutationType> UpdateLastSeen(string firebaseId);
        public Task DeleteUser(string username);
        public Task<UserType> GetById(string firebaseId);
        public Task<IEnumerable<UserType>> GetByIds(IReadOnlyList<string> ids, CancellationToken token = default );
    }
}
