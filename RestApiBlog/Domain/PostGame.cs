using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestApiBlog.Domain
{
    public class PostGame
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(GameId))]
        public virtual Game Game { get; set; }

        public Guid GameId { get; set; }

        [ForeignKey(nameof(PostId))]
        public virtual Post Post { get; set; }

        public Guid PostId { get; set; }
    }
}
