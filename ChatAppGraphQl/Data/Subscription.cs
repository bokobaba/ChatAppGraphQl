using ChatAppGraphQl.DTOs.MessageDtos;
using ChatAppGraphQl.DTOs.UserDtos;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;

namespace ChatAppGraphQl.Data {
    public class Subscription {
        public const string USER_TYPING = "User_Typing";

        [Subscribe]
        public GetMessageDto MessagePosted([EventMessage] GetMessageDto message) => message;

        //[Subscribe]
        //public GetUserDto UserTyping(string username, [EventMessage] GetUserDto user) {
        //    return user;
        //}

        [SubscribeAndResolve]
        public async IAsyncEnumerable<GetUserDto> UserTyping(string username, 
            [Service] ITopicEventReceiver eventReceiver) {
            ISourceStream stream = await eventReceiver.SubscribeAsync<string, GetUserDto>(USER_TYPING);

            await foreach (GetUserDto data in stream.ReadEventsAsync()) {
                if (username != data.Username)
                    yield return data;
            }
        }
    }
}
