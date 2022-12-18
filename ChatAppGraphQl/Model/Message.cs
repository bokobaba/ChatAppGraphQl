using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatAppGraphQl.Model {
    public class Message {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }

        public string Username { get; set; }
        public User User { get; set; }
    }
}
