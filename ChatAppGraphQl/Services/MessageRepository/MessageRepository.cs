using AutoMapper;
using ChatAppGraphQl.Data;
using ChatAppGraphQl.DTOs.MessageDtos;
using ChatAppGraphQl.Model;
using Microsoft.EntityFrameworkCore;

namespace ChatAppGraphQl.Services.MessageRepository {
    public class MessageRepository : IMessageRepository {
        private readonly IDbContextFactory<ApplicationDbContext> _context;
        private readonly IMapper _mapper;

        public MessageRepository(IDbContextFactory<ApplicationDbContext> context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetMessageDto> Create(AddMessageDto request) {
            using (ApplicationDbContext context = _context.CreateDbContext()) {
                Message message = _mapper.Map<Message>(request);

                message.TimeStamp = DateTime.UtcNow;

                context.Messages.Add(message);

                await context.SaveChangesAsync();

                return _mapper.Map<GetMessageDto>(message);
            }   
        }

        public async Task<IEnumerable<GetMessageDto>> GetMessages() {
            using (ApplicationDbContext context = _context.CreateDbContext()) {
                IEnumerable<Message> messages = await context.Messages
                    .AsNoTracking()
                    .ToListAsync();

                return messages.Select(m => _mapper.Map<GetMessageDto>(m));
            }
        }

        public async Task<IEnumerable<GetMessageDto>> GetMessagesByIds(IReadOnlyList<string> users) {
            using (ApplicationDbContext context = _context.CreateDbContext()) {
                List<Message> messages = await context.Messages
                    .Where(m => users.Contains(m.Username))
                    .ToListAsync();

                return messages.Select(m => _mapper.Map<GetMessageDto>(m));
            }
        }
    }
}
