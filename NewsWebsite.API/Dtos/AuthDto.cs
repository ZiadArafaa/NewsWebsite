namespace NewsWebsite.API.Dtos
{
    public class AuthDto
    {
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public bool IsAuthenticated { get; set; }
        public string? ErrorMessage { get; set; }
        public List<string>? Roles  { get; set; }
        public string? Token { get; set; }
        public DateTime ExpireOn { get; set; }
    }
}
