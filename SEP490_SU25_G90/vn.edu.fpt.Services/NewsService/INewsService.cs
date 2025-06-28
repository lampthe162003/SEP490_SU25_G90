using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.NewsService
{
    public interface INewsService
    {
        Task<(List<NewsListInformationResponse>, int)> GetPagedNewsAsync(int page, int pageSize);
        Task<NewsListInformationResponse?> GetNewsByIdAsync(int id);
        Task AddNewsAsync(NewsFormRequest request, int authorId);
        Task<bool> DeleteNewsAsync(int id);
    }
}
