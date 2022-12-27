using AutoMapper;
using ChatAppGraphQl.Queries.CommentQueries;
using ChatAppGraphQl.Queries.UserQueries;
using ChatAppGraphQl.Model;
using ChatAppGraphQl.Queries.CommunityQueries;
using ChatAppGraphQl.Queries.PostQueries;

namespace ChatAppGraphQl {
    public class AutoMapperProfile: Profile {
        public AutoMapperProfile() {
            CreateMap<UserInput, User>();
            CreateMap<User, UserMutationType>();
            CreateMap<User, UserType>()
                .ForMember(u => u.Id, opt => opt.MapFrom(s => s.FirebaseId))
                .ForMember(u => u.SubscriptionIds, opt => opt.MapFrom(s => s.Subscriptions.Select(sub => sub.CommunityName)));

            CreateMap<CreateCommentInput, Comment>();
            CreateMap<Comment, CommentMutationType>();
            CreateMap<Comment, CommentType>()
                .ForMember(c => c.ReplyIds, opt => opt.MapFrom(s => s.Replies.Select(r => r.Id)));

            CreateMap<CreateCommunityInput, Community>();
            CreateMap<Community, CommunityMutationType>();
            CreateMap<Community, CommunityType>()
                .ForMember(c => c.SubscriberIds, opt => opt.MapFrom(c => c.Subscribers.Select(s => s.SubscriberId)))
                .ForMember(c => c.PostIds, opt => opt.MapFrom(s => s.Posts.Select(p => p.Id)));

            CreateMap<CreatePostInput, Post>();
            CreateMap<Post, PostMutationType>();
            CreateMap<Post, PostType>()
                .ForMember(p => p.LikeIds, opt => opt.MapFrom(s => s.Likes.Select(l => l.UserId)))
                .ForMember(p => p.CommentIds, opt => opt.MapFrom(s => s.Comments.Select(c => c.Id)));
        }
    }
}
