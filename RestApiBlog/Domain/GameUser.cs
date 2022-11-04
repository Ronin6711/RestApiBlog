using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestApiBlog.Domain
{
    public class GameUser
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(GameId))]
        public virtual Game Game { get; set; }

        public Guid GameId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual PublicProfile PublicProfile { get; set; }

        public Guid UserId { get; set; }
    }
}
