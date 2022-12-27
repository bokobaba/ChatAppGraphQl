using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatAppGraphQl.Queries.PostQueries;

namespace ChatAppGraphQl.Data.PostData {
    [ExtendObjectType(typeof(Query))]
    public class PostQuery {
        private readonly IMapper _mapper;

        public PostQuery(IMapper mapper) {
            _mapper = mapper;
        }

        [UseDbContext(typeof(ApplicationDbContext))]
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 50)]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<PostType> GetPosts([ScopedService] ApplicationDbContext context) {
            return context.Posts.ProjectTo<PostType>(_mapper.ConfigurationProvider);
        }

        [UseDbContext(typeof(ApplicationDbContext))]
        [UseProjection]
        public IQueryable<PostType> GetPost(
            [ScopedService] ApplicationDbContext context,
            Guid id
        ) {
            return context.Posts
                .Where( p => p.Id == id )
                .ProjectTo<PostType>(_mapper.ConfigurationProvider);
        }
    }
}
