using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestApiBlog.Domain
{
    public class PublicProfile
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        public string NickName { get; set; }

        public string? Discord { get; set; }

        public string? Avatar { get; set; }

        public string Status { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]

        public virtual IdentityUser User { get; set; }

        public virtual List<GameUser> Games { get; set; }
    }
}
