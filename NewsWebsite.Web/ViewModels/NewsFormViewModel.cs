using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace NewsWebsite.Web.ViewModels
{
    public class NewsFormViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        [DisplayName("News")]
        public string NewsDetails { get; set; } = null!;
        public string? Image { get; set; }
        public string? PublicId { get; set; }
        [RequiredIf("Id == 0" ,ErrorMessage = "The Image field is required.")]
        public IFormFile? ImageForm { get; set; } 
        [Remote("DataValidate","News",ErrorMessage = "The Date value is not valid.")]
        public DateTime PublicationDate { get; set; }
        [DisplayName("Author")]
        public int AuthorId { get; set; }
        public IEnumerable<SelectListItem>? Authors { get; set; }
    }
}
