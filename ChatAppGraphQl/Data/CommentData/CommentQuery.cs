using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatAppGraphQl.Queries.CommentQueries;
using HotChocolate.AspNetCore.Authorization;

namespace ChatAppGraphQl.Data.CommentData
{
    [ExtendObjectType(typeof(Query))]
    public class CommentQuery
    {
        private readonly IMapper _mapper;

        public CommentQuery(IMapper mapper)
        {
            _mapper = mapper;
        }

        [UseDbContext(typeof(ApplicationDbContext))]
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 100)]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<CommentType> GetComments([ScopedService] ApplicationDbContext context) {
            return context.Comments.ProjectTo<CommentType>(_mapper.ConfigurationProvider);
        }

        [UseDbContext(typeof(ApplicationDbContext))]
        [UseProjection]
        public IQueryable<CommentType> GetComment(
            [ScopedService] ApplicationDbContext context,
            Guid id
        ) {
            return context.Comments
                .Where(c => c.Id == id)
                .ProjectTo<CommentType>(_mapper.ConfigurationProvider);
        }
    }
}
