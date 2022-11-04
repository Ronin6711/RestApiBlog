using System.ComponentModel.DataAnnotations;

namespace RestApiBlog.Domain
{
    public class Game
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public virtual List<GameUser> GameUsers { get; set; }

    }
}
