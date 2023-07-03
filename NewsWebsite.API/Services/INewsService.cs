using NewsWebsite.API.Models;

namespace NewsWebsite.API.Services
{
    public interface INewsService
    {
        public Task<News> CreateAsync(News news);
        public News Update(News news);
        public News Delete(News news);
        public Task<News?> GetByIdAsync(int id);
        public Task<IEnumerable<News>> GetAvailableAsync();
        public Task<IEnumerable<News>> GetDeletedAsync();
    }
}
