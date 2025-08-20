using AutoMapper;
using Moq;
using NUnit.Framework;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.LearningMaterial;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.LearningMaterialRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningMaterialService;

namespace SEP490_SU25_G90.vn.edu.fpt.Tests.LearningMaterialServiceTests
{
    [TestFixture]
    public class LearningMaterialServiceTests
    {
        private Mock<ILearningMaterialRepository> _learningMaterialRepository;
        private Mock<IMapper> _mapper;
        private Mock<IWebHostEnvironment> _env;
        private LearningMaterialService _learningMaterialService;

        private List<LearningMaterial> _testMaterials;

        [SetUp]
        public void SetUp()
        {
            _learningMaterialRepository = new Mock<ILearningMaterialRepository>();
            _mapper = new Mock<IMapper>();
            _env = new Mock<IWebHostEnvironment>();

            _testMaterials = new List<LearningMaterial>
            {
                new LearningMaterial { MaterialId = 1, Title = "Tài liệu học lái xe B1", Description = "Mô tả B1", LicenceType = new LicenceType { LicenceCode = "B1" }, CreatedAt = DateTime.Now },
                new LearningMaterial { MaterialId = 2, Title = "Tài liệu học lái xe B2", Description = "Mô tả B2", LicenceType = new LicenceType { LicenceCode = "B2" }, CreatedAt = DateTime.Now },
                new LearningMaterial { MaterialId = 3, Title = "Biển báo giao thông", Description = "Mô tả biển báo", LicenceType = new LicenceType { LicenceCode = "B1" }, CreatedAt = DateTime.Now }
            };

            _learningMaterialService = new LearningMaterialService(
                _learningMaterialRepository.Object,
                _mapper.Object,
                _env.Object
            );
        }

        [Test]
        public async Task GetPagedMaterialsAsync_ValidPage_ReturnsCorrectData()
        {
            int page = 1;
            int pageSize = 2;

            _learningMaterialRepository.Setup(x => x.GetPagedMaterialsAsync(page, pageSize))
                .ReturnsAsync((_testMaterials.Take(pageSize).ToList(), _testMaterials.Count));

            _mapper.Setup(m => m.Map<List<LearningMaterialListInformationResponse>>(It.IsAny<List<LearningMaterial>>()))
                .Returns((List<LearningMaterial> source) =>
                    source.Select(lm => new LearningMaterialListInformationResponse
                    {
                        MaterialId = lm.MaterialId,
                        Title = lm.Title ?? "",
                        Description = lm.Description ?? "",
                        LicenceTypeName = lm.LicenceType?.LicenceCode,
                        FileLink = lm.FileLink,
                        CreatedAt = lm.CreatedAt
                    }).ToList());

            var result = await _learningMaterialService.GetPagedMaterialsAsync(page, pageSize);

            Assert.That(result.Item1.Count, Is.EqualTo(2));
            Assert.That(result.Item2, Is.EqualTo(3));
            Assert.That(result.Item1[0].Title, Is.EqualTo("Tài liệu học lái xe B1"));
            Assert.That(result.Item1[1].Title, Is.EqualTo("Tài liệu học lái xe B2"));
        }
        [Test]
        public async Task GetPagedMaterialsAsync_SecondPage_ReturnsCorrectData()
        {
            int page = 2, pageSize = 2;
            _learningMaterialRepository.Setup(x => x.GetPagedMaterialsAsync(page, pageSize))
                .ReturnsAsync((_testMaterials.Skip(pageSize).Take(pageSize).ToList(), _testMaterials.Count));

            var result = await _learningMaterialService.GetPagedMaterialsAsync(page, pageSize);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Item1.Count, Is.EqualTo(1));
            Assert.That(result.Item2, Is.EqualTo(3));
        }

        [Test]
        public async Task GetPagedMaterialsAsync_InvalidPageOrPageSize_ReturnsEmptyList()
        {
            _learningMaterialRepository.Setup(x => x.GetPagedMaterialsAsync(0, 10))
                .ReturnsAsync((new List<LearningMaterial>(), 0));

            _learningMaterialRepository.Setup(x => x.GetPagedMaterialsAsync(1, 0))
                .ReturnsAsync((new List<LearningMaterial>(), 0));

            var resultPageZero = await _learningMaterialService.GetPagedMaterialsAsync(0, 10);
            var resultPageSizeZero = await _learningMaterialService.GetPagedMaterialsAsync(1, 0);

            Assert.That(resultPageZero.Item1.Count, Is.EqualTo(0));
            Assert.That(resultPageZero.Item2, Is.EqualTo(0));

            Assert.That(resultPageSizeZero.Item1.Count, Is.EqualTo(0));
            Assert.That(resultPageSizeZero.Item2, Is.EqualTo(0));
        }

        [Test]
        public async Task GetPagedMaterialsAsync_EmptyRepository_ReturnsEmptyList()
        {
            _learningMaterialRepository.Setup(x => x.GetPagedMaterialsAsync(1, 10))
                .ReturnsAsync((new List<LearningMaterial>(), 0));

            var result = await _learningMaterialService.GetPagedMaterialsAsync(1, 10);

            Assert.That(result.Item1.Count, Is.EqualTo(0));
            Assert.That(result.Item2, Is.EqualTo(0));
        }

        [Test]
        public async Task GetMaterialByIdAsync_ValidId_ReturnsMaterial()
        {
            int id = 1;
            var material = _testMaterials.First();
            _learningMaterialRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(material);
            _mapper.Setup(m => m.Map<LearningMaterialListInformationResponse>(material))
                   .Returns(new LearningMaterialListInformationResponse
                   {
                       MaterialId = material.MaterialId,
                       Title = material.Title,
                       Description = material.Description,
                       LicenceTypeName = material.LicenceType?.LicenceCode,
                       FileLink = material.FileLink,
                       CreatedAt = material.CreatedAt
                   });

            var result = await _learningMaterialService.GetMaterialByIdAsync(id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.MaterialId, Is.EqualTo(id));
        }
        [Test]
        public async Task GetPagedMaterialsAsync_SecondPage_ReturnsRemainingItems()
        {
            // Arrange
            int page = 2, pageSize = 2;

            _learningMaterialRepository.Setup(x => x.GetPagedMaterialsAsync(page, pageSize))
                .ReturnsAsync((_testMaterials.Skip(pageSize).Take(pageSize).ToList(), _testMaterials.Count));

            _mapper.Setup(m => m.Map<List<LearningMaterialListInformationResponse>>(It.IsAny<List<LearningMaterial>>()))
                .Returns((List<LearningMaterial> source) =>
                    source.Select(lm => new LearningMaterialListInformationResponse
                    {
                        MaterialId = lm.MaterialId,
                        Title = lm.Title ?? "",
                        Description = lm.Description ?? "",
                        LicenceTypeName = lm.LicenceType?.LicenceCode,
                        FileLink = lm.FileLink,
                        CreatedAt = lm.CreatedAt
                    }).ToList());

            // Act
            var result = await _learningMaterialService.GetPagedMaterialsAsync(page, pageSize);

            // Assert
            Assert.That(result.Item1.Count, Is.EqualTo(1)); // chỉ còn 1 item ở page 2
            Assert.That(result.Item2, Is.EqualTo(_testMaterials.Count));
        }

        [Test]
        public async Task GetMaterialByIdAsync_InvalidOrZeroId_ReturnsNull()
        {
            _learningMaterialRepository.Setup(x => x.GetByIdAsync(999))
                .ReturnsAsync((LearningMaterial)null);

            _learningMaterialRepository.Setup(x => x.GetByIdAsync(0))
                .ReturnsAsync((LearningMaterial)null);

            var resultInvalid = await _learningMaterialService.GetMaterialByIdAsync(999);
            var resultZero = await _learningMaterialService.GetMaterialByIdAsync(0);

            Assert.That(resultInvalid, Is.Null);
            Assert.That(resultZero, Is.Null);
        }
        
    }
}
