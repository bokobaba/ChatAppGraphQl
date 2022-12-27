using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatAppGraphQl.Queries.CommunityQueries;

namespace ChatAppGraphQl.Data.CommunityData {
    [ExtendObjectType(typeof(Query))]
    public class CommunityQuery {
        private readonly IMapper _mapper;

        public CommunityQuery(IMapper mapper) { 
            _mapper = mapper;
        }

        [UseDbContext(typeof(ApplicationDbContext))]
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 5)]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<CommunityType> GetCommunities([ScopedService] ApplicationDbContext context) {
            return context.Communities.ProjectTo<CommunityType>(_mapper.ConfigurationProvider);
        }

        [UseDbContext(typeof(ApplicationDbContext))]
        [UseProjection]
        public IQueryable<CommunityType> GetCommunity(
            [ScopedService] ApplicationDbContext context,
            string name
        ) {
            return context.Communities
                .Where(c => c.Name == name)
                .ProjectTo<CommunityType>(_mapper.ConfigurationProvider);
        }
    }
}
