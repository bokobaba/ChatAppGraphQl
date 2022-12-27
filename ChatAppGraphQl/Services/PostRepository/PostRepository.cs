using AutoMapper;
using ChatAppGraphQl.Data;
using ChatAppGraphQl.Model;
using ChatAppGraphQl.Queries.Core;
using ChatAppGraphQl.Queries.PostQueries;
using Microsoft.EntityFrameworkCore;

namespace ChatAppGraphQl.Services.PostRepository {
    public class PostRepository : IPostRepository {
        private const string POST_NOT_FOUND_ERROR = "POST_NOT_FOUND";
        private const string ALREADY_LIKED_POST_ERROR = "ALREADY_LIKED_POST";
        private readonly IDbContextFactory<ApplicationDbContext> _context;
        private readonly IMapper _mapper;

        public PostRepository(IDbContextFactory<ApplicationDbContext> context, IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PostMutationType> CreatePost(string firebaseId, CreatePostInput input) {
            using (ApplicationDbContext context = _context.CreateDbContext()) {
                Post post = _mapper.Map<Post>(input);
                post.CreatorId = firebaseId;
                post.Timestamp= DateTime.UtcNow;

                context.Posts.Add(post);
                await context.SaveChangesAsync();

                return _mapper.Map<PostMutationType>(post);
            }
        }

        public async Task<Response> DeletePost(string firebaseId, Guid postId) {
            using (ApplicationDbContext context = _context.CreateDbContext()) {
                Post? post = await context.Posts
                    .FirstOrDefaultAsync(p => p.Id == postId && p.CreatorId == firebaseId);

                if (post == null)
                    throw new PostNotFoundException(postId);

                context.Remove(post);
                await context.SaveChangesAsync();

                return _mapper.Map<Response>(new Response {
                    Message = $"Removed Post with Id = {postId}",
                    Success = true
                });
            }
        }

        public async Task<PostMutationType> EditPost(string firebaseId, EditPostInput input) {
            using (ApplicationDbContext context = _context.CreateDbContext()) {
                Post? post = await context.Posts
                    .FirstOrDefaultAsync(p => p.Id == input.id && p.CreatorId == firebaseId);

                if (post == null)
                    throw new PostNotFoundException(input.id);

                post.Text = input.Text;
                post.Title = input.Title;

                await context.SaveChangesAsync();

                return _mapper.Map<PostMutationType>(post);
            }
        }

        public async Task<IEnumerable<PostType>> GetByIds(IReadOnlyCollection<Guid> ids) {
            using (ApplicationDbContext context = _context.CreateDbContext()) {
                IEnumerable<Post> posts = await context.Posts
                    .AsNoTracking()
                    .Where(p => ids.Contains(p.Id))
                    .ToListAsync();

                return posts.Select(p => _mapper.Map<PostType>(p));
            }
        }

        public async Task<Response> LikePost(string firebaseId, Guid postId) {
            using (ApplicationDbContext context = _context.CreateDbContext()) {
                Post? post = await context.Posts
                    .FirstOrDefaultAsync(p => p.Id == postId);

                if (post == null)
                    throw new PostNotFoundException(postId);

                if (post.Likes.Any(l => l.UserId == firebaseId))
                    throw new GraphQLException(new Error(
                        $"User: {firebaseId} has already liked post: {post.Id}",
                        ALREADY_LIKED_POST_ERROR));

                post.Likes.Add(new UserLikePost {
                    UserId = firebaseId,
                    PostId = post.Id
                });
                
                await context.SaveChangesAsync();

                return _mapper.Map<Response>(new Response {
                    Message = $"User: {firebaseId} has like post: {postId}",
                    Success = true
                });
            }
        }

        private class PostNotFoundException: GraphQLException {
            public PostNotFoundException(Guid id) : base(
                new Error( 
                    $"Post not found with Id: {id}",
                    POST_NOT_FOUND_ERROR
                )
            ) {}
        }
    }
}
