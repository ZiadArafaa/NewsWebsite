using System.ComponentModel.DataAnnotations;

namespace NewsWebsite.API.Dtos
{
    public class LoginDto
    {
        [MaxLength(50)]
        public string UserName { get; set; } = null!;
        [RegularExpression("^(?=.*[!@#$%^&*)(_\\-+=\\[\\]{}\\\\|:;\"'<>,./?])(?=.*[A-Z])(?=.*\\d).+$", ErrorMessage = "Password not storng")]
        public string Password { get; set; } = null!;
    }
}
