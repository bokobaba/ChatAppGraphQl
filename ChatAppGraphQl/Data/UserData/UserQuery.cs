using AutoMapper.QueryableExtensions;
using AutoMapper;
using ChatAppGraphQl.Queries.UserQueries;
using HotChocolate.AspNetCore.Authorization;

namespace ChatAppGraphQl.Data.UserData
{
    [ExtendObjectType(typeof(Query))]
    [Authorize]
    public class UserQuery
    {
        private readonly IMapper _mapper;

        public UserQuery(IMapper mapper)
        {
            _mapper = mapper;
        }

        [UseDbContext(typeof(ApplicationDbContext))]
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 5)]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<UserType> GetUsers([ScopedService] ApplicationDbContext context) {
            return context.Users.ProjectTo<UserType>(_mapper.ConfigurationProvider);
        }

        [UseDbContext(typeof(ApplicationDbContext))]
        [UseProjection]
        public IQueryable<UserType> GetUser(
            [ScopedService] ApplicationDbContext context,
            string id
        ) {
            return context.Users
                .Where(u => u.FirebaseId == id)
                .ProjectTo<UserType>(_mapper.ConfigurationProvider);
        }
    }
}
