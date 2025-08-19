using Moq;
using NUnit.Framework;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Class;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.ClassReponsitory;
using SEP490_SU25_G90.vn.edu.fpt.Services.ClassService;

namespace SEP490_SU25_G90.vn.edu.fpt.Tests.ClassServiceTests
{
    [TestFixture]
    public class ClassServiceTests
    {
        private Mock<IClassRepository> _classRepository;
        private IClassService _classService;
        private List<ClassListResponse> _testClasses;

        [SetUp]
        public void SetUp()
        {
            _classRepository = new Mock<IClassRepository>();

            _testClasses = new List<ClassListResponse>
            {
                new ClassListResponse
                {
                    ClassId = 1,
                    ClassName = "Lớp B1-001",
                    LicenceCode = "B1",
                    InstructorName = "Nguyễn Văn A",
                    StartDate = DateTime.Now.AddDays(10),
                    EndDate = DateTime.Now.AddDays(40),
                    Status = "Đang tuyển sinh",
                    TotalStudents = 15
                },
                new ClassListResponse
                {
                    ClassId = 2,
                    ClassName = "Lớp B2-001",
                    LicenceCode = "B2",
                    InstructorName = "Trần Thị B",
                    StartDate = DateTime.Now.AddDays(5),
                    EndDate = DateTime.Now.AddDays(35),
                    Status = "Đang học",
                    TotalStudents = 20
                },
                new ClassListResponse
                {
                    ClassId = 3,
                    ClassName = "Lớp C-001",
                    LicenceCode = "C",
                    InstructorName = "Phạm Văn C",
                    StartDate = DateTime.Now.AddDays(-10),
                    EndDate = DateTime.Now.AddDays(20),
                    Status = "Đã kết thúc",
                    TotalStudents = 18
                }
            };

            _classService = new ClassService(_classRepository.Object);
        }

        [Test]
        public async Task GetClassesAsync_ValidRequest_ReturnsCorrectPagination()
        {
            // Arrange
            var searchRequest = new ClassSearchRequest
            {
                PageNumber = 1,
                PageSize = 2,
                ClassName = "",
                LicenceTypeId = null,
                Status = null
            };

            _classRepository.Setup(x => x.GetClassesAsync(searchRequest))
                .ReturnsAsync(_testClasses.Take(2).ToList());

            _classRepository.Setup(x => x.CountClassesAsync(searchRequest))
                .ReturnsAsync(3);

            // Act
            var result = await _classService.GetClassesAsync(searchRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data.Count, Is.EqualTo(2));
            Assert.That(result.Total, Is.EqualTo(3));
            Assert.That(result.TotalPage, Is.EqualTo(2));
            Assert.That(result.Data.First().ClassName, Is.EqualTo("Lớp B1-001"));
            Assert.That(result.Data.Skip(1).First().ClassName, Is.EqualTo("Lớp B2-001"));
        }

        [Test]
        public async Task GetClassesAsync_SecondPage_ReturnsCorrectData()
        {
            // Arrange
            var searchRequest = new ClassSearchRequest
            {
                PageNumber = 2,
                PageSize = 2,
                ClassName = "",
                LicenceTypeId = null,
                Status = null
            };

            _classRepository.Setup(x => x.GetClassesAsync(searchRequest))
                .ReturnsAsync(_testClasses.Skip(2).Take(1).ToList());

            _classRepository.Setup(x => x.CountClassesAsync(searchRequest))
                .ReturnsAsync(3);

            // Act
            var result = await _classService.GetClassesAsync(searchRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data.Count, Is.EqualTo(1));
            Assert.That(result.Total, Is.EqualTo(3));
            Assert.That(result.TotalPage, Is.EqualTo(2));
            Assert.That(result.Data.First().ClassName, Is.EqualTo("Lớp C-001"));
        }

        [Test]
        public async Task GetClassesAsync_InvalidPageNumber_DefaultsToPageOne()
        {
            // Arrange
            var searchRequest = new ClassSearchRequest
            {
                PageNumber = 0,
                PageSize = 10,
                ClassName = "",
                LicenceTypeId = null,
                Status = null
            };

            _classRepository.Setup(x => x.GetClassesAsync(It.Is<ClassSearchRequest>(r => r.PageNumber == 1)))
                .ReturnsAsync(_testClasses);

            _classRepository.Setup(x => x.CountClassesAsync(It.Is<ClassSearchRequest>(r => r.PageNumber == 1)))
                .ReturnsAsync(3);

            // Act
            var result = await _classService.GetClassesAsync(searchRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data.Count, Is.EqualTo(3));
            Assert.That(result.Total, Is.EqualTo(3));
            Assert.That(result.TotalPage, Is.EqualTo(1));
        }

        [Test]
        public async Task GetClassesAsync_InvalidPageSize_DefaultsToPageSizeTen()
        {
            // Arrange
            var searchRequest = new ClassSearchRequest
            {
                PageNumber = 1,
                PageSize = 0,
                ClassName = "",
                LicenceTypeId = null,
                Status = null
            };

            _classRepository.Setup(x => x.GetClassesAsync(It.Is<ClassSearchRequest>(r => r.PageSize == 10)))
                .ReturnsAsync(_testClasses);

            _classRepository.Setup(x => x.CountClassesAsync(It.Is<ClassSearchRequest>(r => r.PageSize == 10)))
                .ReturnsAsync(3);

            // Act
            var result = await _classService.GetClassesAsync(searchRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data.Count, Is.EqualTo(3));
            Assert.That(result.Total, Is.EqualTo(3));
            Assert.That(result.TotalPage, Is.EqualTo(1));
        }

        [Test]
        public async Task GetClassesAsync_EmptyResult_ReturnsEmptyPagination()
        {
            // Arrange
            var searchRequest = new ClassSearchRequest
            {
                PageNumber = 1,
                PageSize = 10,
                ClassName = "NonExistentClass",
                LicenceTypeId = null,
                Status = null
            };

            _classRepository.Setup(x => x.GetClassesAsync(searchRequest))
                .ReturnsAsync(new List<ClassListResponse>());

            _classRepository.Setup(x => x.CountClassesAsync(searchRequest))
                .ReturnsAsync(0);

            // Act
            var result = await _classService.GetClassesAsync(searchRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data.Count, Is.EqualTo(0));
            Assert.That(result.Total, Is.EqualTo(0));
            Assert.That(result.TotalPage, Is.EqualTo(0));
        }

        [Test]
        public async Task GetClassesAsync_ExactPageSize_ReturnsCorrectTotalPages()
        {
            // Arrange
            var searchRequest = new ClassSearchRequest
            {
                PageNumber = 1,
                PageSize = 3,
                ClassName = "",
                LicenceTypeId = null,
                Status = null
            };

            _classRepository.Setup(x => x.GetClassesAsync(searchRequest))
                .ReturnsAsync(_testClasses);

            _classRepository.Setup(x => x.CountClassesAsync(searchRequest))
                .ReturnsAsync(3);

            // Act
            var result = await _classService.GetClassesAsync(searchRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data.Count, Is.EqualTo(3));
            Assert.That(result.Total, Is.EqualTo(3));
            Assert.That(result.TotalPage, Is.EqualTo(1));
        }

        [Test]
        public async Task GetClassesAsync_OverflowPageSize_ReturnsCorrectTotalPages()
        {
            // Arrange
            var searchRequest = new ClassSearchRequest
            {
                PageNumber = 1,
                PageSize = 2,
                ClassName = "",
                LicenceTypeId = null,
                Status = null
            };

            _classRepository.Setup(x => x.GetClassesAsync(searchRequest))
                .ReturnsAsync(_testClasses.Take(2).ToList());

            _classRepository.Setup(x => x.CountClassesAsync(searchRequest))
                .ReturnsAsync(3);

            // Act
            var result = await _classService.GetClassesAsync(searchRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data.Count, Is.EqualTo(2));
            Assert.That(result.Total, Is.EqualTo(3));
            Assert.That(result.TotalPage, Is.EqualTo(2));
        }
    }
}
