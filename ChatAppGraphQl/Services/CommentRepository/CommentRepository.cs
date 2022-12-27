using AutoMapper;
using ChatAppGraphQl.Data;
using ChatAppGraphQl.Queries.CommentQueries;
using ChatAppGraphQl.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ChatAppGraphQl.Queries.Core;

namespace ChatAppGraphQl.Services.CommentRepository {
    public class CommentRepository : ICommentRepository {
        private const string COMMENT_NOT_FOUND_ERROR = "COMMENT_NOT_FOUND";
        private readonly IDbContextFactory<ApplicationDbContext> _context;
        private readonly IMapper _mapper;

        public CommentRepository(IDbContextFactory<ApplicationDbContext> context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CommentMutationType> Create(string firebaseId, CreateCommentInput input) {
            using ApplicationDbContext context = _context.CreateDbContext();
            Comment comment = _mapper.Map<Comment>(input);
            comment.TimeStamp = DateTime.UtcNow;
            comment.CreatorId = firebaseId;

            EntityEntry<Comment> added = context.Comments.Add(comment);

            await context.SaveChangesAsync();

            return _mapper.Map<CommentMutationType>(comment);
        }

        public async Task<Response> DeleteAllComments(string username) {
            using ApplicationDbContext context = _context.CreateDbContext();
            IEnumerable<Comment> comments = await context.Comments
                .Include(c => c.Creator)
                .Where(c => c.Creator.Username == username)
                .ToListAsync();

            context.Comments.RemoveRange(comments);
            await context.SaveChangesAsync();

            return new Response {
                Message = $"All messages for User {username} successfully deleted",
                Success = true
            };
        }

        public async Task<Response> DeleteComment(string firebaseId, Guid id) {
            using ApplicationDbContext context = _context.CreateDbContext();
            Comment? comment = await context.Comments
                .FirstOrDefaultAsync(c => c.CreatorId == firebaseId && c.Id == id);

            if (comment == null)
                throw new CommentNotFoundException(id);

            context.Comments.Remove(comment);
            await context.SaveChangesAsync();

            return new Response {
                Message = $"message with id {id} successfully deleted",
                Success = true
            };
        }

        public async Task<IEnumerable<CommentType>> GetByIds(IReadOnlyList<Guid> ids) {
            using ApplicationDbContext context = _context.CreateDbContext();
            IEnumerable<Comment> comments = await context.Comments
                .AsNoTrackingWithIdentityResolution()
                .Include(c => c.Replies)
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();

            return comments.Select(c => _mapper.Map<CommentType>(c));
        }

        private class CommentNotFoundException : GraphQLException {
            public CommentNotFoundException(Guid id) : base(
                new Error(
                    $"Comment not found with id: {id}",
                    COMMENT_NOT_FOUND_ERROR
                )
            ) { }
        }
    }
}
