using ChatAppGraphQl.DTOs.UserDtos;

namespace ChatAppGraphQl.Services.UserRepository {
    public interface IUserRepository {
        public Task<GetUserDto> Create(AddUserDto request);
        public Task<GetUserDto> UpdateLastSeen(string username);
        public Task<GetUserDto> UpdateLastTyped(string username);
    }
}
