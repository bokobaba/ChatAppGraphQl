using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatAppGraphQl.Model {
    public class Community {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? BannerImage { get; set; }

        public List<CommunitySubscription> Subscribers { get; set; } = new List<CommunitySubscription>();

        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
