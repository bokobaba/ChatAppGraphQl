using ChatAppGraphQl.Queries.CommentQueries;
using ChatAppGraphQl.Queries.Core;

namespace ChatAppGraphQl.Services.CommentRepository {
    public interface ICommentRepository {
        public Task<CommentMutationType> Create(string firebaseId, CreateCommentInput input);
        public Task<Response> DeleteAllComments(string username);
        public Task<Response> DeleteComment(string firebaseId, Guid id);
        public Task<IEnumerable<CommentType>> GetByIds(IReadOnlyList<Guid> ids);
    }
}
