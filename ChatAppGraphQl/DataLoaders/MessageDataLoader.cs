using ChatAppGraphQl.DTOs.MessageDtos;
using ChatAppGraphQl.DTOs.UserDtos;
using ChatAppGraphQl.Services.MessageRepository;

namespace ChatAppGraphQl.DataLoaders {
    public class MessageDataLoader : BatchDataLoader<string, GetMessageDto> {
        private readonly IMessageRepository _repository;

        public MessageDataLoader(IMessageRepository repository, IBatchScheduler batchScheduler, 
            DataLoaderOptions options) : base(batchScheduler, options) {
            _repository = repository;
        }

        protected override async Task<IReadOnlyDictionary<string, GetMessageDto>> LoadBatchAsync(
            IReadOnlyList<string> keys, CancellationToken cancellationToken) {
            IEnumerable<GetMessageDto> messages = await _repository.GetMessagesByIds(keys);

            return messages.ToDictionary(m => m.TimeStamp.ToString());
        }
    }
}
