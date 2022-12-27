using ChatAppGraphQl.Queries.CommunityQueries;
using ChatAppGraphQl.Queries.Core;

namespace ChatAppGraphQl.Services.CommunityRepository {
    public interface ICommunityRepository {
        public Task<CommunityMutationType> Create(string firebaseId, CreateCommunityInput input);
        public Task<Response> Subscribe(string firebaseId, string name);
        public Task<Response> UnSubscribe(string firebaseId, string name);
        public Task<IEnumerable<CommunityType>> GetByIds(IReadOnlyList<string> ids);
    }
}
