using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestApiBlog.Domain
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        public string Header { get; set; }

        public string Description { get; set; }

        public string? Image { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual IdentityUser User { get; set; }

        public virtual List<PostGame>? Games { get; set; }
    }
}
