using Microsoft.EntityFrameworkCore;
using NewsWebsite.API.Data;
using NewsWebsite.API.Models;

namespace NewsWebsite.API.Services
{
    public class NewsService : INewsService
    {
        private readonly ApplicationDbContext _context;
        public NewsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<News> CreateAsync(News news)
        {
            await _context.AddAsync(news);
            _context.SaveChanges();

            return news;
        }
        public News Update(News news)
        {
            _context.Update(news);
            _context.SaveChanges();

            return news;
        }
        public News Delete(News news) 
        {
            _context.Remove(news);
            _context.SaveChanges();

            return news;
        }
        public async Task<News?> GetByIdAsync(int id)
        {
            return await _context.News.Include(p=>p.Author).SingleOrDefaultAsync(p=>p.Id ==id);
        }
        public async Task<IEnumerable<News>> GetAvailableAsync()
        {
            return await _context.News.Where(n => !n.IsDeleted).Include(p=>p.Author).ToListAsync();
        }
        public async Task<IEnumerable<News>> GetDeletedAsync()
        {
            return await _context.News.Where(n => n.IsDeleted).Include(p=>p.Author).ToListAsync();
        }
    }
}
