using AutoMapper;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.News;
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
            var (news, totalNews) = await _newsRepository.GetPagedNewsAsync(page, pageSize);

            var result = _mapper.Map<List<NewsListInformationResponse>>(news);
            return (result, totalNews);
        }

        public async Task<NewsListInformationResponse?> GetNewsByIdAsync(int id)
        {
            var news = await _newsRepository.GetNewsByIdAsync(id);
            if (news == null)
                return null;

            return _mapper.Map<NewsListInformationResponse>(news);
        }
        public async Task AddNewsAsync(NewsFormRequest request, int authorId)
        {
            var news = _mapper.Map<News>(request);
            news.AuthorId = authorId;
            news.PostTime = DateTime.Now;
            news.Image = await SaveImageAsync(request.Image);

            await _newsRepository.AddNewsAsync(news);
        }
        public async Task<NewsFormRequest?> GetNewsFormByIdAsync(int id)
        {
            var news = await _newsRepository.GetNewsByIdAsync(id);
            if (news == null) return null;

            return new NewsFormRequest
            {
                NewsId = news.NewsId,
                Title = news.Title,
                NewsContent = news.NewsContent,
                OldImagePath = news.Image
            };
        }
        public async Task<bool> EditNewsAsync(NewsFormRequest request)
        {
            var news = await _newsRepository.GetNewsByIdAsync(request.NewsId);
            if (news == null) return false;

            news.Title = request.Title;
            news.NewsContent = request.NewsContent;
            news.PostTime = DateTime.Now;
            news.Image = await SaveImageAsync(request.Image, request.OldImagePath);

            return await _newsRepository.EditNewsAsync(news);
        }

        public async Task<bool> DeleteNewsAsync(int id)
        {
            var news = await _newsRepository.GetNewsByIdAsync(id);
            if (news == null)
            {
                return false;
            }

            // Xóa ảnh nếu tồn tại
            if (!string.IsNullOrEmpty(news.Image))
            {
                var fullImagePath = Path.Combine(_env.WebRootPath, news.Image.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (System.IO.File.Exists(fullImagePath))
                {
                    System.IO.File.Delete(fullImagePath);
                }
            }

            return await _newsRepository.DeleteNewsAsync(news);
        }

        private async Task<string?> SaveImageAsync(IFormFile? file, string? oldImagePath = null)
        {
            if (file == null) return oldImagePath;

            // Xóa ảnh cũ nếu có
            if (!string.IsNullOrEmpty(oldImagePath))
            {
                var fullOldPath = Path.Combine(_env.WebRootPath, oldImagePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (System.IO.File.Exists(fullOldPath))
                {
                    System.IO.File.Delete(fullOldPath);
                }
            }

            // Dùng tên file gốc
            var originalFileName = Path.GetFileName(file.FileName);
            var folder = Path.Combine(_env.WebRootPath, "uploads", "news");
            Directory.CreateDirectory(folder);

            var path = Path.Combine(folder, originalFileName);
            string uniqueFileName = originalFileName;

            // Nếu file trùng tên → thêm hậu tố (1), (2), ...
            int count = 1;
            string nameOnly = Path.GetFileNameWithoutExtension(originalFileName);
            string extension = Path.GetExtension(originalFileName);
            while (System.IO.File.Exists(path))
            {
                uniqueFileName = $"{nameOnly} ({count++}){extension}";
                path = Path.Combine(folder, uniqueFileName);
            }

            // Save
            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            //Trả về đường dẫn tương đối để lưu DB
            return "/uploads/news/" + uniqueFileName;
        }

        public async Task<IList<NewsListInformationResponse>> GetTopNewsAsync()
        {
            var topNews = await _newsRepository.GetNewsListAsync();
            return _mapper.Map<IList<NewsListInformationResponse>>(topNews.OrderBy(news => news.NewsId).Take(5).ToList());
        }
    }
}

