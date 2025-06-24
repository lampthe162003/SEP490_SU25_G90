using AutoMapper;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.NewsRepository;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.NewsService
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IMapper _mapper;

        public NewsService(INewsRepository newsRepository, IMapper mapper)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
        }

        public async Task<(List<NewsListInformationResponse>, int)> GetPagedNewsAsync(int page, int pageSize)
        {
            var (news, totalItems) = await _newsRepository.GetPagedNewsAsync(page, pageSize);

            var result = _mapper.Map<List<NewsListInformationResponse>>(news);
            return (result, totalItems);
        }

        public async Task<NewsListInformationResponse?> GetNewsByIdAsync(int id)
        {
            var entity = await _newsRepository.GetNewsByIdAsync(id);
            if (entity == null)
                return null;

            return _mapper.Map<NewsListInformationResponse>(entity);
        }
    }
}

