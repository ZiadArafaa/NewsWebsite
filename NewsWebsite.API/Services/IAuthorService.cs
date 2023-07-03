using NewsWebsite.API.Models;

namespace NewsWebsite.API.Services
{
    public interface IAuthorService
    {
        public Task<IEnumerable<Author>> GetAvailableAsync();
        public Task<IEnumerable<Author>> GetDeletedAsync();
        public Task<IEnumerable<Author>> GetAllAsync();
        public Task<Author?> GetByIdAsync(int id);
        public Task<Author?> GetByAvailableIdAsync(int id);
        public Task<Author> CreateAsync(Author author);
        public Author Delete(Author author);
        public Author Update(Author author);
    }
}
