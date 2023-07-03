using Microsoft.EntityFrameworkCore;
using NewsWebsite.API.Data;
using NewsWebsite.API.Models;

namespace NewsWebsite.API.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext _context;
        public AuthorService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Author>> GetAvailableAsync()
        {
            return await _context.Authors.Where(a => !a.IsDeleted).ToListAsync();
        }
        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _context.Authors.ToListAsync();
        }
        public async Task<IEnumerable<Author>> GetDeletedAsync()
        {
            return await _context.Authors.Where(a => a.IsDeleted).ToListAsync();
        }
        public async Task<Author?> GetByIdAsync(int id)
        {
            return await _context.Authors.FindAsync(id);
        }
        public async Task<Author?> GetByAvailableIdAsync(int id)
        {
            return await _context.Authors.SingleOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }
        public async Task<Author> CreateAsync(Author author)
        {
            await _context.AddAsync(author);
            _context.SaveChanges();

            return author;
        }
        public Author Delete(Author author)
        {
            _context.Remove(author);
            _context.SaveChanges();

            return author;
        }
        public Author Update(Author author)
        {
            _context.Update(author);
            _context.SaveChanges();

            return author;
        }
    }
}
