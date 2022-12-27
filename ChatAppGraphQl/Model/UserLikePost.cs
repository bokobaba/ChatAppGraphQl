namespace ChatAppGraphQl.Model {
    public class UserLikePost {
        public string UserId { get; set; }
        public User User { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}
