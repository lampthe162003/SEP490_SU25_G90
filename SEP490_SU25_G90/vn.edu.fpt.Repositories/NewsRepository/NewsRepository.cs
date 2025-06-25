using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.NewsRepository
{
    public class NewsRepository : INewsRepository
    {
        private readonly Sep490Su25G90DbContext _context;

        public NewsRepository(Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public async Task<(List<News>, int)> GetPagedNewsAsync(int page, int pageSize)
        {
            var totalItems = await _context.News.CountAsync();

            var items = await _context.News
                .Include(n => n.Author)
                .OrderByDescending(n => n.PostTime) 
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalItems);
        }
        public async Task<News?> GetNewsByIdAsync(int id)
        {
            return await _context.News
                .Include(n => n.Author)
                .FirstOrDefaultAsync(n => n.NewsId == id);
        }
    }
}
