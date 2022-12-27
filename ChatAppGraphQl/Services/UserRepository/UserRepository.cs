using AutoMapper;
using ChatAppGraphQl.Data;
using ChatAppGraphQl.Queries.UserQueries;
using ChatAppGraphQl.Model;
using Microsoft.EntityFrameworkCore;

namespace ChatAppGraphQl.Services.UserRepository {
    public class UserRepository : IUserRepository {
        private readonly IDbContextFactory<ApplicationDbContext> _context;
        private readonly IMapper _mapper;

        public UserRepository(IDbContextFactory<ApplicationDbContext> context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserType> GetById(string firebaseId) {
            using (ApplicationDbContext context = _context.CreateDbContext()) {
                User? user = await context.Users.FirstOrDefaultAsync(u => u.FirebaseId == firebaseId);

                if (user == null)
                    throw new UserNotFoundException(firebaseId);

                return _mapper.Map<UserType>(user);
            }
        }

        public async Task<IEnumerable<UserType>> GetByIds(IReadOnlyList<string> ids, 
            CancellationToken token = default) {
            using (ApplicationDbContext context = _context.CreateDbContext()) {
                IEnumerable<User> users = await context.Users
                    .AsNoTracking()
                    .Where(u => ids.Contains(u.FirebaseId))
                    .ToListAsync();

                return users.Select(u => _mapper.Map<UserType>(u));
            }
        }

        public async Task<UserMutationType> Create(string firebaseId, UserInput request) {
            using (ApplicationDbContext context = _context.CreateDbContext()) {
                User user = _mapper.Map<User>(request);

                user.LastSeen = DateTime.UtcNow;
                user.FirebaseId = firebaseId;
                context.Users.Add(user);
                await context.SaveChangesAsync();

                return _mapper.Map<UserMutationType>(user);
            }
        }

        public async Task<UserMutationType> UpdateLastSeen(string firebaseId) {
            using (ApplicationDbContext context = _context.CreateDbContext()) {
                User? user = await context.Users
                    .Where(x => x.FirebaseId == firebaseId)
                    .FirstOrDefaultAsync();

                if (user == null)
                    throw new UserNotFoundException(firebaseId);

                user.LastSeen = DateTime.UtcNow;
                await context.SaveChangesAsync();

                return _mapper.Map<UserMutationType>(user);
            }
        }

        public async Task DeleteUser(string username) {
            using (ApplicationDbContext context = _context.CreateDbContext()) {
                User? user = await context.Users
                    .Where(x => x.Username == username)
                    .FirstOrDefaultAsync();

                if (user == null)
                    throw new UserNotFoundException(username);

                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }

        private class UserNotFoundException : GraphQLException {
            public UserNotFoundException(string str) : base(
                new Error(
                    $"User not found matching: {str}",
                    "USER_NOT_FOUND"
                )
            ) { }
        }
    }
}
