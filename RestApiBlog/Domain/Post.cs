using System.ComponentModel.DataAnnotations;

namespace RestApiBlog.Domain
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
