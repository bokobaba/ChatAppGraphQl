using ChatAppGraphQl.DTOs.MessageDtos;

namespace ChatAppGraphQl.Services.MessageRepository {
    public interface IMessageRepository {
        public Task<GetMessageDto> Create(AddMessageDto request);
        public Task<IEnumerable<GetMessageDto>> GetMessages();
        public Task<IEnumerable<GetMessageDto>> GetMessagesByIds(IReadOnlyList<string> users);
    }
}
