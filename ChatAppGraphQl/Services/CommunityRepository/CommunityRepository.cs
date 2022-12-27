using AutoMapper;
using ChatAppGraphQl.Data;
using ChatAppGraphQl.Model;
using ChatAppGraphQl.Queries.CommunityQueries;
using ChatAppGraphQl.Queries.Core;
using Microsoft.EntityFrameworkCore;

namespace ChatAppGraphQl.Services.CommunityRepository {
    public class CommunityRepository : ICommunityRepository {
        private const string ALREADY_SUBSCRIBED_ERROR = "ALREADY_SUBSCRIBED";
        private const string COMMUNITY_NOT_FOUND_ERROR = "COMMUNITY_NOT_FOUND";
        private const string NOT_SUBSCRIBED_ERROR = "NOT_SUBSCRIBED";

        private readonly IDbContextFactory<ApplicationDbContext> _context;
        private readonly IMapper _mapper;

        public CommunityRepository(IDbContextFactory<ApplicationDbContext> context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CommunityMutationType> Create(string firebaseId, CreateCommunityInput input) {
            Community community = _mapper.Map<Community>(input);

            using (ApplicationDbContext context = _context.CreateDbContext()) {
                community.Subscribers.Add(new CommunitySubscription {
                    SubscriberId = firebaseId,
                    CommunityName = input.Name,
                });

                context.Add(community);

                await context.SaveChangesAsync();

                return _mapper.Map<CommunityMutationType>(community);
            }
        }

        public async Task<IEnumerable<CommunityType>> GetByIds(IReadOnlyList<string> ids) {
            using (ApplicationDbContext context = _context.CreateDbContext()) {
                IEnumerable<Community> communities = await context.Communities
                    .AsNoTracking()
                    .Where(c => ids.Contains(c.Name))
                    .ToListAsync();

                return communities.Select(c => _mapper.Map<CommunityType>(c));
            }
        }

        public async Task<Response> Subscribe(string firebaseId, string name) {
            using (ApplicationDbContext context = _context.CreateDbContext()) {
                Community? community = await context.Communities
                    .Include(c => c.Subscribers)
                    .FirstOrDefaultAsync(c => c.Name == name);

                if (community == null)
                    throw new CommunityNotFoundException(name);

                if (community.Subscribers.Any(c => c.SubscriberId == firebaseId))
                    throw new GraphQLException(
                        new Error($"{firebaseId} is Already Subscribed to {name}", 
                        ALREADY_SUBSCRIBED_ERROR));

                community.Subscribers.Add(new CommunitySubscription {
                    SubscriberId = firebaseId,
                    CommunityName = community.Name,
                });

                await context.SaveChangesAsync();

                return new Response {
                    Success = true,
                    Message = $"{firebaseId} is now subscribed to {name}"
                };
            }
        }

        public async Task<Response> UnSubscribe(string firebaseId, string name) {
            using (ApplicationDbContext context = _context.CreateDbContext()) {
                Community? community = await context.Communities
                    .Include(c => c.Subscribers)
                    .FirstOrDefaultAsync(c => c.Name == name);

                if (community == null)
                    throw new CommunityNotFoundException(name);

                int removed = community.Subscribers.RemoveAll(s => s.SubscriberId == firebaseId);

                if (removed != 1)
                    throw new GraphQLException(
                        new Error($"{firebaseId} is not subscribed to {name}",
                        NOT_SUBSCRIBED_ERROR));

                await context.SaveChangesAsync();

                return new Response {
                    Success = true,
                    Message = $"{firebaseId} is now unsubscribed from {name}"
                };
            }
        }

        private class CommunityNotFoundException : GraphQLException {
            public CommunityNotFoundException(string str) : base(
                new Error(
                    $"Community not found matching: {str}",
                    COMMUNITY_NOT_FOUND_ERROR
                )
            ) { }
        }
    }
}
