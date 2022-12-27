using ChatAppGraphQl.Queries.Core;
using ChatAppGraphQl.Queries.PostQueries;

namespace ChatAppGraphQl.Services.PostRepository {
    public interface IPostRepository {
        public Task<PostMutationType> CreatePost(string firebaseId, CreatePostInput input);
        public Task<IEnumerable<PostType>> GetByIds(IReadOnlyCollection<Guid> ids);
        public Task<Response> DeletePost(string firebaseId, Guid postId);
        public Task<Response> LikePost(string firebaseId, Guid postId);
        public Task<PostMutationType> EditPost(string firebaseId, EditPostInput input);
    }
}
