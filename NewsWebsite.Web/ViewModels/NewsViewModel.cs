using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewsWebsite.Web.ViewModels
{
    public class NewsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string NewsDetails { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string PublicId { get; set; } = null!;
        public DateTime PublicationDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string AuthorName { get; set; } = null!;
    }
}
