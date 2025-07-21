using SEP490_SU25_G90.vn.edu.fpt.Models;

namespace SEP490_SU25_G90.vn.edu.fpt.Repositories.NewsRepository
{
    public interface INewsRepository
    {
        Task<(List<News>, int)> GetPagedNewsAsync(int page, int pageSize);
        Task<News?> GetNewsByIdAsync(int id);
        Task AddNewsAsync(News news);
        Task<bool> EditNewsAsync(News news);
        Task<bool> DeleteNewsAsync(News news);
        Task<IList<News>> GetNewsListAsync();
    }
}
