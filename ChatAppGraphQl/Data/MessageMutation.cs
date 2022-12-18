using ChatAppGraphQl.DTOs.MessageDtos;
using ChatAppGraphQl.Services.MessageRepository;
using HotChocolate.Subscriptions;

namespace ChatAppGraphQl.Data {
    [ExtendObjectType(typeof(Mutation))]
    public class MessageMutation {
        private readonly IMessageRepository _repository;

        public MessageMutation(IMessageRepository repository) {
            _repository = repository;
        }

        public async Task<GetMessageDto> PostMessage(AddMessageDto message,
            [Service] ITopicEventSender sender) {
            GetMessageDto response = await _repository.Create(message);

            await sender.SendAsync(nameof(Subscription.MessagePosted), response);

            return response;
        }
    }
}
