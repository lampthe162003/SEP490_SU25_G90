using AutoMapper;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.LearningMaterial;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.LearningMaterialRepository;

namespace SEP490_SU25_G90.vn.edu.fpt.Services.LearningMaterialService
{
    public class LearningMaterialService : ILearningMaterialService
    {
        private readonly ILearningMaterialRepository _iLearningMaterialRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public LearningMaterialService(ILearningMaterialRepository iLearningMaterialRepository, IMapper mapper, IWebHostEnvironment env)
        {
            _iLearningMaterialRepository = iLearningMaterialRepository;
            _mapper = mapper;
            _env = env;
        }

        public async Task<(List<LearningMaterialListInformationResponse>, int)> GetPagedMaterialsAsync(int page, int pageSize)
        {
            var (learningMaterial, totalLearningMaterial) = await _iLearningMaterialRepository.GetPagedMaterialsAsync(page, pageSize);
            var result = _mapper.Map<List<LearningMaterialListInformationResponse>>(learningMaterial);
            return (result, totalLearningMaterial);
        }

        public async Task<LearningMaterialListInformationResponse?> GetMaterialByIdAsync(int id)
        {
            var learningMaterial = await _iLearningMaterialRepository.GetByIdAsync(id);
            return _mapper.Map<LearningMaterialListInformationResponse>(learningMaterial);
        }
        public async Task<List<LicenceType>> GetLicenceTypesAsync()
        {
            return await _iLearningMaterialRepository.GetLicenceTypesAsync();
        }
        public async Task AddMaterialAsync(LearningMaterialFormRequest request)
        {
            var material = _mapper.Map<LearningMaterial>(request);
            material.CreatedAt = DateTime.Now;
            material.FileLink = await SaveFileAsync(request.File);
            await _iLearningMaterialRepository.AddAsync(material);
        }
        public async Task<LearningMaterialFormRequest?> GetFormByIdAsync(int id)
        {
            var material = await _iLearningMaterialRepository.GetByIdAsync(id);
            if (material == null) return null;

            return new LearningMaterialFormRequest
            {
                MaterialId = material.MaterialId,
                Title = material.Title ?? "",
                Description = material.Description ?? "",
                LicenceTypeId = material.LicenceTypeId ?? 0,
                OldFilePath = material.FileLink
            };
        }
        public async Task<bool> EditMaterialAsync(LearningMaterialFormRequest request)
        {
            var material = await _iLearningMaterialRepository.GetByIdAsync(request.MaterialId);
            if (material == null) return false;

            _mapper.Map(request, material);
            material.FileLink = await SaveFileAsync(request.File, request.OldFilePath);
            material.CreatedAt = DateTime.Now;

            return await _iLearningMaterialRepository.EditMaterialAsync(material);
        }
        public async Task<bool> DeleteLearningMaterialAsync(int id)
        {
            var material = await _iLearningMaterialRepository.GetByIdAsync(id);
            if (material == null)
                return false;

            
            if (!string.IsNullOrEmpty(material.FileLink))
            {
                var fullPath = Path.Combine(_env.WebRootPath, material.FileLink.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            return await _iLearningMaterialRepository.DeleteMaterialAsync(material);
        }

        private async Task<string?> SaveFileAsync(IFormFile? file, string? oldFilePath = null)
        {
            if (file == null) return oldFilePath;

            // Xoá file cũ nếu có
            if (!string.IsNullOrEmpty(oldFilePath))
            {
                var fullOldPath = Path.Combine(_env.WebRootPath, oldFilePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (System.IO.File.Exists(fullOldPath))
                {
                    System.IO.File.Delete(fullOldPath);
                }
            }

            // ✅ Lấy tên gốc của file
            var originalFileName = Path.GetFileName(file.FileName);
            var folder = Path.Combine(_env.WebRootPath, "uploads", "learningmaterial");
            Directory.CreateDirectory(folder);

            // Đường dẫn file mặc định
            var path = Path.Combine(folder, originalFileName);
            string uniqueFileName = originalFileName;

            // Nếu file đã tồn tại -> thêm hậu tố
            int count = 1;
            string fileNameOnly = Path.GetFileNameWithoutExtension(originalFileName);
            string extension = Path.GetExtension(originalFileName);
            while (System.IO.File.Exists(path))
            {
                uniqueFileName = $"{fileNameOnly} ({count++}){extension}";
                path = Path.Combine(folder, uniqueFileName);
            }

            // Lưu file mới
            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            // ✅ Trả về đường dẫn tương đối
            return "/uploads/learningmaterial/" + uniqueFileName;
        }
    }
}
