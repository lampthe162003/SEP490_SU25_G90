﻿using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.News;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.NewsService
{
    public interface INewsService
    {
        Task<(List<NewsListInformationResponse>, int)> GetPagedNewsAsync(int page, int pageSize);
        Task<NewsListInformationResponse?> GetNewsByIdAsync(int id);
        Task AddNewsAsync(NewsFormRequest request, int authorId);
        Task<NewsFormRequest?> GetNewsFormByIdAsync(int id);
        Task<bool> EditNewsAsync(NewsFormRequest request);
        Task<bool> DeleteNewsAsync(int id);
        Task<IList<NewsListInformationResponse>> GetTopNewsAsync();
    }
}
