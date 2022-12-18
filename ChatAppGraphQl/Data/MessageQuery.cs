using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatAppGraphQl.DTOs.MessageDtos;
using ChatAppGraphQl.Model;
using ChatAppGraphQl.Services.MessageRepository;
using Microsoft.EntityFrameworkCore;

namespace ChatAppGraphQl.Data {
    [ExtendObjectType(typeof(Query))]
    public class MessageQuery {
        private readonly IMapper _mapper;

        public MessageQuery(IMapper mapper) {
            _mapper = mapper;
        }

        [UseDbContext(typeof(ApplicationDbContext))]
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 5)]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<GetMessageDto> GetMessages([ScopedService] ApplicationDbContext context) {
            return context.Messages.ProjectTo<GetMessageDto>(_mapper.ConfigurationProvider);
        }
    }
}
