using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace NewsWebsite.API.Dtos
{
    public class NewsDto
    {
        public string Title { get; set; } = null!;
        public string NewsDetails { get; set; } = null!;
        public string? Image { get; set; }
        public string? PublicId { get; set; }
        [AssertThat("PublicationDate >= Today() && PublicationDate < Today()+TimeSpan(7,0,0,0)")]
        public DateTime PublicationDate { get; set; }
        public int AuthorId { get; set; }
    }
}
