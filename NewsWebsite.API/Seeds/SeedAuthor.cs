using NewsWebsite.API.Models;
using NewsWebsite.API.Services;

namespace NewsWebsite.API.Seeds
{
    public class SeedAuthor
    {
        private readonly IAuthorService _authorService;
        public SeedAuthor(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        public async Task SeedAsync()
        {
            if (!(await _authorService.GetAllAsync()).Any())
            {
                Author author = new() { Name = "Admin", Email = "Admin@Author.com" };
                await _authorService.CreateAsync(author);
            }
        }
    }
}
