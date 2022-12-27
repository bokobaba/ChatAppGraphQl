using ChatAppGraphQl.Model;

namespace ChatAppGraphQl.Queries.CommentQueries {
    public record CreateCommentInput(Guid postId, string text, Guid? replyToId) { }
}
