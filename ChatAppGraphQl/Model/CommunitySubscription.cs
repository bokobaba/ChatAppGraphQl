namespace ChatAppGraphQl.Model {
    public class CommunitySubscription {
        public string SubscriberId { get; set; }
        public User Subscriber { get; set; }
        public string CommunityName { get; set; }
        public Community Community { get; set; }
    }
}
