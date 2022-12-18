using ChatAppGraphQl.Model;

namespace ChatAppGraphQl.DTOs.UserDtos {
    public class GetUserDto {
        public string Username { get; set; }
        public DateTime LastSeen { get; set; }
        public DateTime LastTyped { get; set; }
    }
}
