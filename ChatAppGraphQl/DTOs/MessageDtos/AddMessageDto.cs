using ChatAppGraphQl.Model;

namespace ChatAppGraphQl.DTOs.MessageDtos {
    public class AddMessageDto {
        public string Text { get; set; }
        public string Username { get; set; }
    }
}
