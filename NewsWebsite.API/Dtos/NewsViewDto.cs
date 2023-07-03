namespace NewsWebsite.API.Dtos
{
    public class NewsViewDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string NewsDetails { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string PublicId { get; set; } = null!;
        public DateTime PublicationDate { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = null!;
    }
}
