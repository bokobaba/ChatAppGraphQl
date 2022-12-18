using AutoMapper;
using ChatAppGraphQl.Data;
using ChatAppGraphQl.DTOs.UserDtos;
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

        public async Task<GetUserDto> Create(AddUserDto request) {
            using (ApplicationDbContext context = _context.CreateDbContext()) {
                User user = _mapper.Map<User>(request);

                user.LastSeen = DateTime.UtcNow;
                context.Users.Add(user);
                await context.SaveChangesAsync();

                return _mapper.Map<GetUserDto>(user);
            }
        }

        public async Task<GetUserDto> UpdateLastSeen(string username) {
            using (ApplicationDbContext context = _context.CreateDbContext()) {
                User? user = await context.Users
                    .Where(x => x.Username == username)
                    .FirstOrDefaultAsync();

                if (user == null)
                    throw new UserNotFoundException(username);

                user.LastSeen = DateTime.UtcNow;
                await context.SaveChangesAsync();

                return _mapper.Map<GetUserDto>(user);
            }
        }
        public async Task<GetUserDto> UpdateLastTyped(string username) {
            using (ApplicationDbContext context = _context.CreateDbContext()) {
                User? user = await context.Users
                    .Where(x => x.Username == username)
                    .FirstOrDefaultAsync();

                if (user == null)
                    throw new UserNotFoundException(username);

                user.LastTyped = DateTime.UtcNow;
                await context.SaveChangesAsync();

                return _mapper.Map<GetUserDto>(user);
            }
        }

        private class UserNotFoundException: GraphQLException {
            public UserNotFoundException(string username) : base(
                new Error(
                    String.Format("User not found with username: {0}", username),
                    "USER_NOT_FOUND"
                )
            ) { }
        }
    }
}
