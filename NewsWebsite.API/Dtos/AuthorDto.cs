using System.ComponentModel.DataAnnotations;

namespace NewsWebsite.API.Dtos
{
    public class AuthorDto
    {
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string Name { get; set; } = null!;
        [EmailAddress, MaxLength(50)]
        public string Email { get; set; } = null!;
    }
}
