using AutoMapper;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.NewsRepository;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.NewsService
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public NewsService(INewsRepository newsRepository, IMapper mapper, IWebHostEnvironment env)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
            _env = env;
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
        public async Task AddNewsAsync(NewsFormRequest request, int authorId)
        {
            var news = _mapper.Map<News>(request);
            news.AuthorId = authorId;
            news.PostTime = DateTime.Now;
            news.Image = await SaveImageAsync(request.Image);

            await _newsRepository.AddNewsAsync(news);
        }
        private async Task<string?> SaveImageAsync(IFormFile? file)
        {
            if (file == null) return null;

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var folder = Path.Combine(_env.WebRootPath, "uploads", "news");
            Directory.CreateDirectory(folder);

            var path = Path.Combine(folder, fileName);
            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return "/uploads/news/" + fileName;
        }
    }
}

