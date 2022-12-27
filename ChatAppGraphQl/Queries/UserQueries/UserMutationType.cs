namespace ChatAppGraphQl.Queries.UserQueries {
    public class UserMutationType {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public DateTime LastSeen { get; set; }
    }
}
