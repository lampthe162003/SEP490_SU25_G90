using AutoMapper;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.UserRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.UserService;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.InMemory;

namespace SEP490_SU25_G90.vn.edu.fpt.Tests.UserServiceTests
{
    [TestFixture]
    public class GetAllUsersUnitTest
    {
        private Mock<IUserRepository> _userRepository;
        private IUserService _userService;
        private Mock<IMapper> _mapper;
        private List<Models.User> _testUsers;

        [SetUp]
        public void SetUp()
        {
            _userRepository = new Mock<IUserRepository>();
            _mapper = new Mock<IMapper>();

            _testUsers = new List<Models.User>()
            {
                new User
                {
                    UserId = 101,
                    Email = "test.learner@example.com",
                    Password = "test_data_1",
                    FirstName = "Trần",
                    MiddleName = "Văn",
                    LastName = "A",
                    Dob = DateOnly.FromDateTime(DateTime.Parse("1996-01-09")),
                    Gender = true,
                    Phone = "0123456789",
                    UserRoles = new List<UserRole>
                    {
                        new UserRole
                        {
                            UserRoleId = 101,
                            UserId = 101,
                            RoleId = 1
                        }
                    }
                },

                new User
                {
                    UserId = 102,
                    Email = "test.hr@example.com",
                    Password = "test_data_2",
                    FirstName = "Nguyễn",
                    MiddleName = "Thị",
                    LastName = "B",
                    Dob = DateOnly.FromDateTime(DateTime.Parse("1997-02-10")),
                    Gender = false,
                    Phone = "0111222333",
                    UserRoles = new List<UserRole>
                    {
                        new UserRole
                        {
                            UserRoleId = 102,
                            UserId = 102,
                            RoleId = 2
                        }
                    }
                },

                new User
                {
                    UserId = 103,
                    Email = "test.instructor@example.com",
                    Password = "test_data_3",
                    FirstName = "Phạm",
                    MiddleName = "Nhật",
                    LastName = "C",
                    Dob = DateOnly.FromDateTime(DateTime.Parse("1995-03-11")),
                    Gender = true,
                    Phone = "0444555666",
                    UserRoles = new List<UserRole>
                    {
                        new UserRole
                        {
                            UserRoleId = 103,
                            UserId = 103,
                            RoleId = 3
                        }
                    }
                },

                new User
                {
                    UserId = 104,
                    Email = "test.academicaffairs@example.com",
                    Password = "test_data_4",
                    FirstName = "Nguyễn",
                    MiddleName = "Thị",
                    LastName = "D",
                    Dob = DateOnly.FromDateTime(DateTime.Parse("1995-04-12")),
                    Gender = true,
                    Phone = "0777888999",
                    UserRoles = new List<UserRole>
                    {
                        new UserRole
                        {
                            UserRoleId = 104,
                            UserId = 104,
                            RoleId = 4
                        }
                    }
                }
            };

            _userRepository.Setup(x => x.GetAllUsers()).ReturnsAsync(_testUsers);

            _mapper.Setup(m => m.Map<List<UserListInformationResponse>>(It.IsAny<List<Models.User>>()))
                .Returns((List<Models.User> source) =>
                    source.Select(u => new UserListInformationResponse
                    {
                        UserId = u.UserId,
                        Email = u.Email,
                        FullName = $"{u.FirstName} {u.MiddleName} {u.LastName}".Trim(),
                        Gender = u.Gender,
                        Dob = u.Dob,
                        Phone = u.Phone
                    }).ToList());

            _userService = new UserService(
                context: null,
                mapper: _mapper.Object,
                userRepository: _userRepository.Object,
                hasher: null,
                roleRepository: null,
                env: null,
                emailService: null
            );
        }

        [Test]
        public async Task GetAllUsers_NoFilters_ReturnsAllUsers()
        {
            var result = await _userService.GetAllUsers(null, null);

            Assert.That(result.Count, Is.EqualTo(4));
            Assert.That(result[0].Email, Is.EqualTo("test.learner@example.com"));
            Assert.That(result[1].FullName, Is.EqualTo("Nguyễn Thị B"));
            Assert.That(result[2].Phone, Is.EqualTo("0444555666"));
            Assert.That(result[3].Phone, Is.EqualTo("0777888999"));
        }

        [Test]
        public async Task GetAllUsers_WithNameFilter_ReturnsFilteredData()
        {
            var result = await _userService.GetAllUsers("Trần Văn", null);

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].FullName, Is.EqualTo("Trần Văn A"));
        }

        [Test]
        public async Task GetAllUsers_WithEmailFilter_ReturnsFilteredData()
        {
            var result = await _userService.GetAllUsers(null, "instructor");

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Email, Is.EqualTo("test.instructor@example.com"));
        }

        [Test]
        public async Task GetAllUsers_WithBothFilters_ReturnFilteredData()
        {
            var result = await _userService.GetAllUsers("Trần Văn A", "test.learner@example.com");

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Dob, Is.EqualTo(DateOnly.FromDateTime(DateTime.Parse("1996-01-09"))));
        }

        [Test]
        public async Task GetAllUsers_WithBothFilters_AndWhiteSpaceName_ReturnFilteredData()
        {
            var result = await _userService.GetAllUsers(" ", "test.instructor@example.com");

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].FullName, Is.EqualTo("Phạm Nhật C"));
        }

        [Test]
        public async Task GetAllUsers_WithBothFilters_AndWhiteSpaceEmail_ReturnFilteredData()
        {
            var result = await _userService.GetAllUsers("Trần Văn A", " ");

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Email, Is.EqualTo("test.learner@example.com"));
        }

        [Test]
        public async Task GetAllUsers_WithBothFilters_AndNoMatchingData_ReturnNoData()
        {
            var result = await _userService.GetAllUsers("Trần Văn A", "nguyen.thi.mai");
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetAllUsers_WithBothFilters_AndAllWhiteSpaces_ReturnFilteredData()
        {
            var result = await _userService.GetAllUsers(" ", " ");

            Assert.That(result.Count, Is.EqualTo(4));
            Assert.That(result[0].Email, Is.EqualTo("test.learner@example.com"));
            Assert.That(result[1].FullName, Is.EqualTo("Nguyễn Thị B"));
            Assert.That(result[2].Phone, Is.EqualTo("0444555666"));
            Assert.That(result[3].Phone, Is.EqualTo("0777888999"));
        }
    }
}
