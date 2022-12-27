using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChatAppGraphQl.Queries.CommentQueries;
using ChatAppGraphQl.Queries.UserQueries;
using ChatAppGraphQl.Queries.SearchQueries;
using HotChocolate.Data.Projections.Expressions;
using Microsoft.EntityFrameworkCore;

namespace ChatAppGraphQl.Data {
    public class Query {
        private readonly IMapper _mapper;

        public Query(IMapper mapper) {
            _mapper = mapper;
        }

        [UseDbContext(typeof(ApplicationDbContext))]
        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 5)]
        [UseProjection]
        [UseFiltering]
        public IEnumerable<ISearchResult> Search(string term,
            [ScopedService] ApplicationDbContext context) {
            IQueryable<ISearchResult> messages = context.Comments
                //.Where(m => m.Text.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Where(m => EF.Functions.ILike(m.Text, $"%{term}%"))
                .ProjectTo<CommentType>(_mapper.ConfigurationProvider);

            IQueryable<ISearchResult> users = context.Users
                                //.Where(u => u.Username.Contains(term, StringComparison.OrdinalIgnoreCase))
                .Where(u => EF.Functions.ILike(u.Username, $"%{term}%"))
                .ProjectTo<UserType>(_mapper.ConfigurationProvider);

            return messages.AsEnumerable().Concat(users.AsEnumerable());
        }
    }
}
