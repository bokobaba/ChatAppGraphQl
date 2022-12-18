using ChatAppGraphQl.Model;

namespace ChatAppGraphQl.DTOs.MessageDtos {
    public class GetMessageDto {
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }

        public string Username { get; set; }
    }
}
