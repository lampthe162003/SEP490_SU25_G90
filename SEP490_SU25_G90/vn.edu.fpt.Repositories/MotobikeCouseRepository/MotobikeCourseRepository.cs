using Microsoft.EntityFrameworkCore;
using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.MotobikeCouseRepository
{
    public class MotobikeCourseRepository : IMotobikeCourseRepository
    {
        private readonly Sep490Su25G90DbContext _context;

        public MotobikeCourseRepository(Sep490Su25G90DbContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetAllMotobikeCourseAsync(string? search)
        {
            var query = _context.Courses.Where(c => c.Title != null && c.Title.ToLower().StartsWith("a"));

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(c => c.Title.ToLower().Contains(search)
                                      || c.Description.ToLower().Contains(search));
            }

            return await query.ToListAsync();
        }
    }
}
