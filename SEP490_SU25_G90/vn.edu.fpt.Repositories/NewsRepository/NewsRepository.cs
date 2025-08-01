﻿using Microsoft.EntityFrameworkCore;
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

        // Lấy danh sách tin tức có phân trang
        public async Task<(List<News>, int)> GetPagedNewsAsync(int page, int pageSize)
        {
            var totalNews = await _context.News.CountAsync();

            var news = await _context.News
                .Include(n => n.Author)
                .OrderByDescending(n => n.PostTime) 
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (news, totalNews);
        }

        // Lấy một bản tin theo ID (bao gồm thông tin tác giả)
        public async Task<News?> GetNewsByIdAsync(int id)
        {
            return await _context.News
                .Include(n => n.Author)
                .FirstOrDefaultAsync(n => n.NewsId == id);
        }

        // Thêm một bản tin mới
        public async Task AddNewsAsync(News news)
        {
            _context.News.Add(news);
            await _context.SaveChangesAsync();
        }

        // Cập nhật thông tin một bản tin
        public async Task<bool> EditNewsAsync(News news)
        {
            _context.News.Update(news);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        // Xóa một bản tin
        public async Task<bool> DeleteNewsAsync(News news)
        {
            _context.News.Remove(news);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        // Lấy danh sách toàn bộ tin tức (không phân trang, không include Author)
        public async Task<IList<News>> GetNewsListAsync()
        {
            return await _context.News.ToListAsync();
        }
    }
}
