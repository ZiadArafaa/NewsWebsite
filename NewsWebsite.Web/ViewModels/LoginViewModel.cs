using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewsWebsite.Web.ViewModels
{
    public class LoginViewModel
    {
        [MaxLength(50)]
        [DisplayName("Usename")]
        public string UserName { get; set; } = null!;
        [RegularExpression("^(?=.*[!@#$%^&*)(_\\-+=\\[\\]{}\\\\|:;\"'<>,./?])(?=.*[A-Z])(?=.*\\d).+$",ErrorMessage ="Password not valid")]
        public string Password { get; set; } = null!;
    }
}
