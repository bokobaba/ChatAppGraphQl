using ChatAppGraphQl.Middleware;
using ChatAppGraphQl.Queries.CommunityQueries;
using ChatAppGraphQl.Queries.Core;
using ChatAppGraphQl.Services.CommunityRepository;
using HotChocolate.AspNetCore.Authorization;

namespace ChatAppGraphQl.Data.CommunityData {
    [ExtendObjectType(typeof(Mutation))]
    [Authorize]
    public class CommunityMutation {
        private readonly ICommunityRepository _repository;

        public CommunityMutation(ICommunityRepository repository) {
            _repository = repository;
        }

        [UseUser]
        public async Task<CommunityMutationType> CreateCommunity(
            CreateCommunityInput community,
            [User] string firebaseId
        ) => await _repository.Create(firebaseId, community);

        [UseUser]
        public async Task<Response> SubscribeToCommunity(
            string name,
            [User] string firebaseId
        ) => await _repository.Subscribe(firebaseId, name);

        [UseUser]
        public async Task<Response> UnSubscribeFromCommunity(
            string name,
            [User] string firebaseId
        ) => await _repository.UnSubscribe(firebaseId, name);
    }
}
