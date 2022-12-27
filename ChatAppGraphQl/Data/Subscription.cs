using ChatAppGraphQl.Queries.CommentQueries;
using ChatAppGraphQl.Queries.UserQueries;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;

namespace ChatAppGraphQl.Data {
    public class Subscription {
        public const string USER_TYPING = "User_Typing";

        [Subscribe]
        public CommentType CommentPosted([EventMessage] CommentType message) => message;

        //[Subscribe]
        //public GetUserDto UserTyping(string username, [EventMessage] GetUserDto user) {
        //    return user;
        //}

        [SubscribeAndResolve]
        public async IAsyncEnumerable<UserType> UserTyping(string username, 
            [Service] ITopicEventReceiver eventReceiver) {
            ISourceStream stream = await eventReceiver.SubscribeAsync<string, UserType>(USER_TYPING);

            await foreach (UserType data in stream.ReadEventsAsync()) {
                if (username != data.Username)
                    yield return data;
            }
        }
    }
}
