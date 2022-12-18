using ChatAppGraphQl.DTOs.UserDtos;
using ChatAppGraphQl.Services.UserRepository;
using HotChocolate.Subscriptions;

namespace ChatAppGraphQl.Data {
    [ExtendObjectType(typeof(Mutation))]
    public class UserMutation {
        private readonly IUserRepository _repository;

        public UserMutation(IUserRepository repository) {
            _repository = repository;
        }

        public async Task<GetUserDto> CreateUser(AddUserDto user) => 
            await _repository.Create(user);

        public async Task<GetUserDto> UpdateLastSeen(string username) =>
            await _repository.UpdateLastSeen(username);

        public async Task<GetUserDto> UpdateLastTyped(string username, [Service] ITopicEventSender sender) {
            GetUserDto response = await _repository.UpdateLastTyped(username);

            await sender.SendAsync(Subscription.USER_TYPING, response);

            return response;
        }
    }
}
