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

        // Lấy danh sách tin tức theo phân trang
        public async Task<(List<NewsListInformationResponse>, int)> GetPagedNewsAsync(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return (new List<NewsListInformationResponse>(), 0);
            }

            var (news, totalNews) = await _newsRepository.GetPagedNewsAsync(page, pageSize);

            if (news == null || !news.Any())
            {
                return (new List<NewsListInformationResponse>(), totalNews);
            }

            var result = news.Select(x => new NewsListInformationResponse
            {
                NewsId = x.NewsId,
                Title = x.Title ?? string.Empty,
                NewsContent = x.NewsContent ?? string.Empty,
                AuthorName = x.Author == null
                    ? string.Empty
                    : $"{x.Author.FirstName} {x.Author.LastName}".Trim(),
                PostTime = x.PostTime,
                ShortContent = string.IsNullOrEmpty(x.NewsContent)
                    ? string.Empty
                    : (x.NewsContent.Length > 100
                        ? x.NewsContent.Substring(0, 100) + "..."
                        : x.NewsContent),
                Image = x.Image ?? string.Empty
            }).ToList();

            return (result, totalNews);
        }


        // Lấy thông tin chi tiết tin tức theo ID
        public async Task<NewsListInformationResponse?> GetNewsByIdAsync(int id)
        {
            var news = await _newsRepository.GetNewsByIdAsync(id);
            if (news == null)
                return null;

            return _mapper.Map<NewsListInformationResponse>(news);
        }

        // Thêm mới tin tức vào hệ thống
        public async Task AddNewsAsync(NewsFormRequest request, int authorId)
        {
            var news = _mapper.Map<News>(request);
            news.AuthorId = authorId;
            news.PostTime = DateTime.Now;
            news.Image = await SaveImageAsync(request.Image); // Lưu ảnh nếu có

            await _newsRepository.AddNewsAsync(news);
        }

        // Lấy thông tin tin tức (NewsFormRequest) theo ID để hiển thị lên giao diện sửa
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

        // Cập nhật thông tin tin tức
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

        // Xóa tin tức theo ID (kèm xóa ảnh nếu có)
        public async Task<bool> DeleteNewsAsync(int id)
        {
            var news = await _newsRepository.GetNewsByIdAsync(id);
            if (news == null)
            {
                return false;
            }

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

        // Lưu ảnh tin tức lên /wwwroot/uploads/news, tránh trùng tên bằng cách thêm hậu tố (1), (2), ...
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

            //Trả về đường dẫn -> lưu DB
            return "/uploads/news/" + uniqueFileName;
        }

        public async Task<IList<NewsListInformationResponse>> GetTopNewsAsync()
        {
            var topNews = await _newsRepository.GetNewsListAsync();
            return _mapper.Map<IList<NewsListInformationResponse>>(topNews.OrderBy(news => news.NewsId).Take(5).ToList());
        }
    }
}

