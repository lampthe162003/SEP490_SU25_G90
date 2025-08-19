using Moq;
using NUnit.Framework;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.LearningApplicationsRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.LearningApplicationsService;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace SEP490_SU25_G90.vn.edu.fpt.Tests.LearningApplicationServiceTests
{
    [TestFixture]
    public class LearningApplicationServiceTests
    {
        private Mock<ILearningApplicationRepository> _learningApplicationRepository;
        private ILearningApplicationService _learningApplicationService;
        private List<LearningApplication> _testLearningApplications;
        private List<LearningApplicationsResponse> _testResponses;

        [SetUp]
        public void SetUp()
        {
            _learningApplicationRepository = new Mock<ILearningApplicationRepository>();

            _testLearningApplications = new List<LearningApplication>
            {
                new LearningApplication
                {
                    LearningId = 1,
                    Learner = new User
                    {
                        UserId = 101,
                        FirstName = "Nguyễn",
                        MiddleName = "Văn",
                        LastName = "A",
                        Cccd = new Cccd { CccdNumber = "123456789012" }
                    },
                                         LicenceType = new LicenceType { LicenceTypeId = 1, LicenceCode = "B1" },
                     LearningStatus = 1
                },
                new LearningApplication
                {
                    LearningId = 2,
                    Learner = new User
                    {
                        UserId = 102,
                        FirstName = "Trần",
                        MiddleName = "Thị",
                        LastName = "B",
                        Cccd = new Cccd { CccdNumber = "987654321098" }
                    },
                                         LicenceType = new LicenceType { LicenceTypeId = 2, LicenceCode = "B2" },
                     LearningStatus = 2
                },
                new LearningApplication
                {
                    LearningId = 3,
                    Learner = new User
                    {
                        UserId = 103,
                        FirstName = "Phạm",
                        MiddleName = "Nhật",
                        LastName = "C",
                        Cccd = new Cccd { CccdNumber = "456789123456" }
                    },
                                         LicenceType = new LicenceType { LicenceTypeId = 1, LicenceCode = "B1" },
                     LearningStatus = 1
                }
            };

            _testResponses = new List<LearningApplicationsResponse>
            {
                new LearningApplicationsResponse
                {
                    LearningId = 1,
                    LearnerFullName = "Nguyễn Văn A",
                    LearnerCccdNumber = "123456789012",
                    LicenceTypeName = "B1",
                    LearningStatus = 1
                },
                new LearningApplicationsResponse
                {
                    LearningId = 2,
                    LearnerFullName = "Trần Thị B",
                    LearnerCccdNumber = "987654321098",
                    LicenceTypeName = "B2",
                    LearningStatus = 2
                },
                new LearningApplicationsResponse
                {
                    LearningId = 3,
                    LearnerFullName = "Phạm Nhật C",
                    LearnerCccdNumber = "456789123456",
                    LicenceTypeName = "B1",
                    LearningStatus = 1
                }
            };

            _learningApplicationService = new LearningApplicationService(_learningApplicationRepository.Object);
        }

        [Test]
        public async Task GetAllAsync_NoFilters_ReturnsAllApplications()
        {
            // Arrange
            _learningApplicationRepository.Setup(x => x.GetAllAsync(null, null))
                .ReturnsAsync(_testResponses);

            // Act
            var result = await _learningApplicationService.GetAllAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result[0].LearnerFullName, Is.EqualTo("Nguyễn Văn A"));
            Assert.That(result[1].LearnerFullName, Is.EqualTo("Trần Thị B"));
            Assert.That(result[2].LearnerFullName, Is.EqualTo("Phạm Nhật C"));
        }

        [Test]
        public async Task GetAllAsync_WithSearchString_ReturnsFilteredApplications()
        {
            // Arrange
            string searchString = "Nguyễn";
            var filteredResponses = _testResponses.Where(r => r.LearnerFullName.Contains(searchString)).ToList();

            _learningApplicationRepository.Setup(x => x.GetAllAsync(searchString, null))
                .ReturnsAsync(filteredResponses);

            // Act
            var result = await _learningApplicationService.GetAllAsync(searchString);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].LearnerFullName, Is.EqualTo("Nguyễn Văn A"));
        }

        [Test]
        public async Task GetAllAsync_WithStatusFilter_ReturnsFilteredApplications()
        {
            // Arrange
            int statusFilter = 1;
            var filteredResponses = _testResponses.Where(r => r.LearningStatus == statusFilter).ToList();

            _learningApplicationRepository.Setup(x => x.GetAllAsync(null, statusFilter))
                .ReturnsAsync(filteredResponses);

            // Act
            var result = await _learningApplicationService.GetAllAsync(null, statusFilter);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.All(r => r.LearningStatus == 1), Is.True);
        }

        [Test]
        public async Task GetAllAsync_WithBothFilters_ReturnsFilteredApplications()
        {
            // Arrange
            string searchString = "B1";
            int statusFilter = 1;
            var filteredResponses = _testResponses.Where(r => r.LicenceTypeName.Contains(searchString) && r.LearningStatus == statusFilter).ToList();

            _learningApplicationRepository.Setup(x => x.GetAllAsync(searchString, statusFilter))
                .ReturnsAsync(filteredResponses);

            // Act
            var result = await _learningApplicationService.GetAllAsync(searchString, statusFilter);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.All(r => r.LicenceTypeName == "B1" && r.LearningStatus == 1), Is.True);
        }

        [Test]
        public async Task GetAllAsync_EmptyResult_ReturnsEmptyList()
        {
            // Arrange
            string searchString = "NonExistent";
            _learningApplicationRepository.Setup(x => x.GetAllAsync(searchString, null))
                .ReturnsAsync(new List<LearningApplicationsResponse>());

            // Act
            var result = await _learningApplicationService.GetAllAsync(searchString);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task FindByCCCD_ValidCCCD_ReturnsMatchingApplications()
        {
            // Arrange
            string cccd = "123456789012";
            var matchingApplications = _testLearningApplications.Where(la => la.Learner.Cccd.CccdNumber == cccd).ToList();

            _learningApplicationRepository.Setup(x => x.GetAllAsync())
                .ReturnsAsync(matchingApplications.AsQueryable());

            // Act
            var result = await _learningApplicationService.FindByCCCD(cccd);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].LearningId, Is.EqualTo(1));
        }

        [Test]
        public async Task FindByCCCD_WithAdditionalFilter_ReturnsFilteredApplications()
        {
            // Arrange
            string cccd = "123456789012";
                         Expression<Func<LearningApplication, bool>> additional = la => la.LearningStatus == 1;
             var matchingApplications = _testLearningApplications.Where(la => 
                 la.Learner.Cccd.CccdNumber == cccd && la.LearningStatus == 1).ToList();

            _learningApplicationRepository.Setup(x => x.GetAllAsync())
                .ReturnsAsync(matchingApplications.AsQueryable());

            // Act
            var result = await _learningApplicationService.FindByCCCD(cccd, additional);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].LearningId, Is.EqualTo(1));
        }

        [Test]
        public async Task FindByCCCD_InvalidCCCD_ReturnsEmptyList()
        {
            // Arrange
            string cccd = "999999999999";
            _learningApplicationRepository.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<LearningApplication>().AsQueryable());

            // Act
            var result = await _learningApplicationService.FindByCCCD(cccd);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task FindByCCCD_EmptyCCCD_ReturnsEmptyList()
        {
            // Arrange
            string cccd = "";
            _learningApplicationRepository.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<LearningApplication>().AsQueryable());

            // Act
            var result = await _learningApplicationService.FindByCCCD(cccd);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task FindLearnerByCccdAsync_ValidCCCD_ReturnsLearnerApplication()
        {
            // Arrange
            string cccd = "123456789012";
            var expectedResponse = new LearningApplicationsResponse
            {
                LearningId = 1,
                LearnerFullName = "Nguyễn Văn A",
                LearnerCccdNumber = "123456789012"
            };

            _learningApplicationRepository.Setup(x => x.FindLearnerByCccdAsync(cccd))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _learningApplicationService.FindLearnerByCccdAsync(cccd);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.LearningId, Is.EqualTo(1));
        }

        [Test]
        public async Task FindLearnerByCccdAsync_InvalidCCCD_ReturnsNull()
        {
            // Arrange
            string cccd = "999999999999";
            _learningApplicationRepository.Setup(x => x.FindLearnerByCccdAsync(cccd))
                .ReturnsAsync((LearningApplicationsResponse)null);

            // Act
            var result = await _learningApplicationService.FindLearnerByCccdAsync(cccd);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task FindLearnerByCccdAsync_EmptyCCCD_ReturnsNull()
        {
            // Arrange
            string cccd = "";
            _learningApplicationRepository.Setup(x => x.FindLearnerByCccdAsync(cccd))
                .ReturnsAsync((LearningApplicationsResponse)null);

            // Act
            var result = await _learningApplicationService.FindLearnerByCccdAsync(cccd);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task FindLearnerByCccdAsync_NullCCCD_ReturnsNull()
        {
            // Arrange
            string cccd = null;
            _learningApplicationRepository.Setup(x => x.FindLearnerByCccdAsync(cccd))
                .ReturnsAsync((LearningApplicationsResponse)null);

            // Act
            var result = await _learningApplicationService.FindLearnerByCccdAsync(cccd);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
