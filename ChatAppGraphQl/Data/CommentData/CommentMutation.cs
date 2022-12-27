using AppAny.HotChocolate.FluentValidation;
using ChatAppGraphQl.Queries.CommentQueries;
using ChatAppGraphQl.Middleware;
using ChatAppGraphQl.Services.CommentRepository;
using ChatAppGraphQl.Validators;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Subscriptions;
using ChatAppGraphQl.Queries.Core;

namespace ChatAppGraphQl.Data.CommentData
{
    [ExtendObjectType(typeof(Mutation))]
    [Authorize]
    public class CommentMutation
    {
        private readonly ICommentRepository _repository;

        public CommentMutation(ICommentRepository repository)
        {
            _repository = repository;
        }

        [UseUser]
        public async Task<CommentMutationType> PostComment(
            [UseFluentValidation, UseValidator<PostMessageInputValidator>] CreateCommentInput message,
            [Service] ITopicEventSender sender,
            [User] string firebaseId
        )
        {
            CommentMutationType response = await _repository.Create(firebaseId, message);

            await sender.SendAsync(nameof(Subscription.CommentPosted), response);

            return response;
        }

        [Authorize(Policy = "IsAdmin")]
        public async Task<Response> DeleteAllComments(string username) =>
            await _repository.DeleteAllComments(username);

        [UseUser]
        public async Task<Response> DeleteComment(
            [User] string firebaseId,
            Guid id
        ) => await _repository.DeleteComment(firebaseId, id);
    }
}
