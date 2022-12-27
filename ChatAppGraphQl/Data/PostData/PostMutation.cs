using ChatAppGraphQl.Middleware;
using ChatAppGraphQl.Queries.Core;
using ChatAppGraphQl.Queries.PostQueries;
using ChatAppGraphQl.Services.PostRepository;
using HotChocolate.AspNetCore.Authorization;

namespace ChatAppGraphQl.Data.PostData {
    [Authorize]
    [ExtendObjectType(typeof(Mutation))]
    public class PostMutation {
        private readonly IPostRepository _repository;

        public PostMutation(IPostRepository repository) {
            _repository = repository;
        }

        [UseUser]
        public async Task<PostMutationType> CreatePost(
            [User] string firebaseId,
            CreatePostInput post
        ) => await _repository.CreatePost(firebaseId, post);

        [UseUser]
        public async Task<PostMutationType> EditPost(
            [User] string firebaseId,
            EditPostInput post
        ) => await _repository.EditPost(firebaseId, post);

        [UseUser]
        public async Task<Response> DeletePost(
            [User] string firebaseId,
            Guid id
        ) => await _repository.DeletePost(firebaseId, id);

        [UseUser]
        public async Task<Response> LikePost(
            [User] string firebaseId,
            Guid id
        ) => await _repository.LikePost(firebaseId, id);
    }
}
