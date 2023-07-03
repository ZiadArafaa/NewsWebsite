using System.ComponentModel.DataAnnotations;

namespace NewsWebsite.API.Models
{
    public class Author
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string Name { get; set; } = null!;
        [EmailAddress,MaxLength(50)]
        public string Email { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}
