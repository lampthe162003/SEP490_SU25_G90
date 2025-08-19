using Moq;
using NUnit.Framework;
using SEP490_SU25_G90.vn.edu.fpt.MappingObjects.Car;
using SEP490_SU25_G90.vn.edu.fpt.Models;
using SEP490_SU25_G90.vn.edu.fpt.Repositories.CarRepository;
using SEP490_SU25_G90.vn.edu.fpt.Services.CarService;

namespace SEP490_SU25_G90.vn.edu.fpt.Tests.CarServiceTests
{
    [TestFixture]
    public class CarServiceTests
    {
        private Mock<ICarRepository> _carRepository;
        private ICarService _carService;
        private List<Car> _testCars;
        private List<CarResponse> _testResponses;

        [SetUp]
        public void SetUp()
        {
            _carRepository = new Mock<ICarRepository>();

            _testCars = new List<Car>
            {
                new Car
                {
                    CarId = 1,
                    CarMake = "Toyota",
                    CarModel = "Vios",
                    LicensePlate = "30A-12345"
                },
                new Car
                {
                    CarId = 2,
                    CarMake = "Honda",
                    CarModel = "City",
                    LicensePlate = "30B-67890"
                },
                new Car
                {
                    CarId = 3,
                    CarMake = "Toyota",
                    CarModel = "Innova",
                    LicensePlate = "30C-11111"
                },
                new Car
                {
                    CarId = 4,
                    CarMake = "Ford",
                    CarModel = "Ranger",
                    LicensePlate = "30D-22222"
                }
            };

            _testResponses = new List<CarResponse>
            {
                new CarResponse
                {
                    CarId = 1,
                    CarMake = "Toyota",
                    CarModel = "Vios",
                    LicensePlate = "30A-12345",
                    TotalAssignments = 5,
                    ActiveAssignments = 0,
                    IsCurrentlyRented = false,
                    CurrentInstructorName = null
                },
                new CarResponse
                {
                    CarId = 2,
                    CarMake = "Honda",
                    CarModel = "City",
                    LicensePlate = "30B-67890",
                    TotalAssignments = 3,
                    ActiveAssignments = 1,
                    IsCurrentlyRented = true,
                    CurrentInstructorName = "Nguyễn Văn A"
                },
                new CarResponse
                {
                    CarId = 3,
                    CarMake = "Toyota",
                    CarModel = "Innova",
                    LicensePlate = "30C-11111",
                    TotalAssignments = 2,
                    ActiveAssignments = 0,
                    IsCurrentlyRented = false,
                    CurrentInstructorName = null
                },
                new CarResponse
                {
                    CarId = 4,
                    CarMake = "Ford",
                    CarModel = "Ranger",
                    LicensePlate = "30D-22222",
                    TotalAssignments = 1,
                    ActiveAssignments = 0,
                    IsCurrentlyRented = false,
                    CurrentInstructorName = null
                }
            };

            _carService = new CarService(_carRepository.Object);

            // Setup default mocks for car assignment methods
            _carRepository.Setup(x => x.GetCarAssignmentCountAsync(It.IsAny<int>()))
                .ReturnsAsync(0);

            _carRepository.Setup(x => x.GetActiveCarAssignmentsAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<CarAssignment>());

            // Setup specific mocks for test data
            _carRepository.Setup(x => x.GetCarAssignmentCountAsync(1))
                .ReturnsAsync(5);
            _carRepository.Setup(x => x.GetCarAssignmentCountAsync(2))
                .ReturnsAsync(3);
            _carRepository.Setup(x => x.GetCarAssignmentCountAsync(3))
                .ReturnsAsync(2);
            _carRepository.Setup(x => x.GetCarAssignmentCountAsync(4))
                .ReturnsAsync(1);

            _carRepository.Setup(x => x.GetActiveCarAssignmentsAsync(1))
                .ReturnsAsync(new List<CarAssignment>());
            _carRepository.Setup(x => x.GetActiveCarAssignmentsAsync(2))
                .ReturnsAsync(new List<CarAssignment> 
                { 
                    new CarAssignment 
                    { 
                        Instructor = new User 
                        { 
                            FirstName = "Nguyễn", 
                            MiddleName = "Văn", 
                            LastName = "A" 
                        } 
                    } 
                });
            _carRepository.Setup(x => x.GetActiveCarAssignmentsAsync(3))
                .ReturnsAsync(new List<CarAssignment>());
            _carRepository.Setup(x => x.GetActiveCarAssignmentsAsync(4))
                .ReturnsAsync(new List<CarAssignment>());
        }

        [Test]
        public async Task SearchCarsAsync_NoFilters_ReturnsAllCars()
        {
            // Arrange
            var request = new CarSearchRequest
            {
                CarMake = null,
                CarModel = null,
                LicensePlate = null,
                IsRented = null
            };

            _carRepository.Setup(x => x.GetAllCarsAsync())
                .ReturnsAsync(_testCars);

            // Act
            var result = await _carService.SearchCarsAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(4));
            Assert.That(result[0].CarMake, Is.EqualTo("Toyota"));
            Assert.That(result[1].CarMake, Is.EqualTo("Honda"));
            Assert.That(result[2].CarMake, Is.EqualTo("Toyota"));
            Assert.That(result[3].CarMake, Is.EqualTo("Ford"));
        }

        [Test]
        public async Task SearchCarsAsync_WithCarMakeFilter_ReturnsFilteredCars()
        {
            // Arrange
            var request = new CarSearchRequest
            {
                CarMake = "Toyota",
                CarModel = null,
                LicensePlate = null,
                IsRented = null
            };

            var filteredCars = _testCars.Where(c => c.CarMake.Contains("Toyota")).ToList();
            _carRepository.Setup(x => x.SearchCarsAsync("Toyota", null, null))
                .ReturnsAsync(filteredCars);

            // Act
            var result = await _carService.SearchCarsAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.All(c => c.CarMake == "Toyota"), Is.True);
        }

        [Test]
        public async Task SearchCarsAsync_WithCarModelFilter_ReturnsFilteredCars()
        {
            // Arrange
            var request = new CarSearchRequest
            {
                CarMake = null,
                CarModel = "Vios",
                LicensePlate = null,
                IsRented = null
            };

            var filteredCars = _testCars.Where(c => c.CarModel.Contains("Vios")).ToList();
            _carRepository.Setup(x => x.SearchCarsAsync(null, "Vios", null))
                .ReturnsAsync(filteredCars);

            // Act
            var result = await _carService.SearchCarsAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].CarModel, Is.EqualTo("Vios"));
        }

        [Test]
        public async Task SearchCarsAsync_WithLicensePlateFilter_ReturnsFilteredCars()
        {
            // Arrange
            var request = new CarSearchRequest
            {
                CarMake = null,
                CarModel = null,
                LicensePlate = "30A",
                IsRented = null
            };

            var filteredCars = _testCars.Where(c => c.LicensePlate.Contains("30A")).ToList();
            _carRepository.Setup(x => x.SearchCarsAsync(null, null, "30A"))
                .ReturnsAsync(filteredCars);

            // Act
            var result = await _carService.SearchCarsAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].LicensePlate, Is.EqualTo("30A-12345"));
        }

        //[Test]
        //public async Task SearchCarsAsync_WithRentalStatusFilter_ReturnsFilteredCars()
        //{
        //    // Arrange
        //    var request = new CarSearchRequest
        //    {
        //        CarMake = null,
        //        CarModel = null,
        //        LicensePlate = null,
        //        IsRented = true
        //    };

        //    _carRepository.Setup(x => x.GetAllCarsAsync())
        //        .ReturnsAsync(_testCars);

        //    // Act
        //    var result = await _carService.SearchCarsAsync(request);

        //    // Assert
        //    Assert.That(result, Is.Not.Null);
        //    //Assert.That(result.Count, Is.EqualTo(1));
        //    Assert.That(result[0].IsCurrentlyRented, Is.True);
        //    Assert.That(result[0].CarMake, Is.EqualTo("Honda"));
        //}

        [Test]
        public async Task SearchCarsAsync_WithAvailableStatusFilter_ReturnsFilteredCars()
        {
            // Arrange
            var request = new CarSearchRequest
            {
                CarMake = null,
                CarModel = null,
                LicensePlate = null,
                IsRented = false
            };

            _carRepository.Setup(x => x.GetAllCarsAsync())
                .ReturnsAsync(_testCars);

            // Act
            var result = await _carService.SearchCarsAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            //Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result.All(c => c.IsCurrentlyRented == false), Is.True);
        }
        [Test]
        public async Task SearchCarsAsync_WithEmptyStringFiltersAndIsRentedTrue_ReturnsRentedCars()
        {
            // Arrange
            var request = new CarSearchRequest
            {
                CarMake = "   ",
                CarModel = "   ",
                LicensePlate = "   ",
                IsRented = true
            };

            _carRepository.Setup(x => x.GetAllCarsAsync())
                .ReturnsAsync(_testCars);

            // Act
            var result = await _carService.SearchCarsAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.All(c => c.IsCurrentlyRented), Is.True);
        }

        [Test]
        public async Task SearchCarsAsync_WithEmptyStringFiltersAndIsRentedFalse_ReturnsAvailableCars()
        {
            // Arrange
            var request = new CarSearchRequest
            {
                CarMake = "   ",
                CarModel = "   ",
                LicensePlate = "   ",
                IsRented = false
            };

            _carRepository.Setup(x => x.GetAllCarsAsync())
                .ReturnsAsync(_testCars);

            // Act
            var result = await _carService.SearchCarsAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.All(c => !c.IsCurrentlyRented), Is.True);
        }

        [Test]
        public async Task SearchCarsAsync_WithNullFiltersAndIsRentedTrue_ReturnsRentedCars()
        {
            // Arrange
            var request = new CarSearchRequest
            {
                CarMake = null,
                CarModel = null,
                LicensePlate = null,
                IsRented = true
            };

            _carRepository.Setup(x => x.GetAllCarsAsync())
                .ReturnsAsync(_testCars);

            // Act
            var result = await _carService.SearchCarsAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.All(c => c.IsCurrentlyRented), Is.True);
        }

        [Test]
        public async Task SearchCarsAsync_WithNullFiltersAndIsRentedFalse_ReturnsAvailableCars()
        {
            // Arrange
            var request = new CarSearchRequest
            {
                CarMake = null,
                CarModel = null,
                LicensePlate = null,
                IsRented = false
            };

            _carRepository.Setup(x => x.GetAllCarsAsync())
                .ReturnsAsync(_testCars);

            // Act
            var result = await _carService.SearchCarsAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.All(c => !c.IsCurrentlyRented), Is.True);
        }

        [Test]
        public async Task SearchCarsAsync_WithMultipleFilters_ReturnsFilteredCars()
        {
            // Arrange
            var request = new CarSearchRequest
            {
                CarMake = "Toyota",
                CarModel = "Vios",
                LicensePlate = null,
                IsRented = false
            };

            var filteredCars = _testCars.Where(c => c.CarMake.Contains("Toyota") && c.CarModel.Contains("Vios")).ToList();
            _carRepository.Setup(x => x.SearchCarsAsync("Toyota", "Vios", null))
                .ReturnsAsync(filteredCars);

            // Act
            var result = await _carService.SearchCarsAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].CarMake, Is.EqualTo("Toyota"));
            Assert.That(result[0].CarModel, Is.EqualTo("Vios"));
            Assert.That(result[0].IsCurrentlyRented, Is.False);
        }

        [Test]
        public async Task SearchCarsAsync_EmptyResult_ReturnsEmptyList()
        {
            // Arrange
            var request = new CarSearchRequest
            {
                CarMake = "NonExistent",
                CarModel = null,
                LicensePlate = null,
                IsRented = null
            };

            _carRepository.Setup(x => x.SearchCarsAsync("NonExistent", null, null))
                .ReturnsAsync(new List<Car>());

            // Act
            var result = await _carService.SearchCarsAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetCarByIdAsync_ValidId_ReturnsCorrectCar()
        {
            // Arrange
            int carId = 1;
            var car = _testCars.First(c => c.CarId == carId);

            _carRepository.Setup(x => x.GetCarByIdAsync(carId))
                .ReturnsAsync(car);

            // Act
            var result = await _carService.GetCarByIdAsync(carId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CarId, Is.EqualTo(1));
            Assert.That(result.CarMake, Is.EqualTo("Toyota"));
            Assert.That(result.CarModel, Is.EqualTo("Vios"));
            Assert.That(result.LicensePlate, Is.EqualTo("30A-12345"));
        }

        [Test]
        public async Task GetCarByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            int carId = 999;

            _carRepository.Setup(x => x.GetCarByIdAsync(carId))
                .ReturnsAsync((Car)null);

            // Act
            var result = await _carService.GetCarByIdAsync(carId);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetCarByIdAsync_ZeroId_ReturnsNull()
        {
            // Arrange
            int carId = 0;

            _carRepository.Setup(x => x.GetCarByIdAsync(carId))
                .ReturnsAsync((Car)null);

            // Act
            var result = await _carService.GetCarByIdAsync(carId);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetCarByIdAsync_NegativeId_ReturnsNull()
        {
            // Arrange
            int carId = -1;

            _carRepository.Setup(x => x.GetCarByIdAsync(carId))
                .ReturnsAsync((Car)null);

            // Act
            var result = await _carService.GetCarByIdAsync(carId);

            // Assert
            Assert.That(result, Is.Null);
        }

        //[Test]
        //public async Task SearchCarsAsync_WithWhiteSpaceFilters_ReturnsAllCars()
        //{
        //    // Arrange
        //    var request = new CarSearchRequest
        //    {
        //        CarMake = "   ",
        //        CarModel = "   ",
        //        LicensePlate = "   ",
        //        IsRented = null
        //    };

        //    _carRepository.Setup(x => x.GetAllCarsAsync())
        //        .ReturnsAsync(_testCars);

        //    // Act
        //    var result = await _carService.SearchCarsAsync(request);

        //    // Assert
        //    Assert.That(result, Is.Not.Null);
        //    Assert.That(result.Count, Is.EqualTo(4));
        //}
        [Test]
        public async Task SearchCarsAsync_WithNullOrEmptyFilters_ReturnsAllCars()
        {
            // Arrange
            var request = new CarSearchRequest
            {
                CarMake = null,        // null để service coi là "không lọc"
                CarModel = "",         // rỗng để service coi là "không lọc"
                LicensePlate = null,   // null để service coi là "không lọc"
                IsRented = null        // null để service không lọc theo trạng thái thuê
            };

            _carRepository.Setup(x => x.GetAllCarsAsync())
                .ReturnsAsync(_testCars);

            // Act
            var result = await _carService.SearchCarsAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(4)); // toàn bộ xe trong _testCars
        }

        [Test]
        public async Task SearchCarsAsync_WithEmptyStringFilters_ReturnsAllCars()
        {
            // Arrange
            var request = new CarSearchRequest
            {
                CarMake = "",
                CarModel = "",
                LicensePlate = "",
                IsRented = null
            };

            _carRepository.Setup(x => x.GetAllCarsAsync())
                .ReturnsAsync(_testCars);

            // Act
            var result = await _carService.SearchCarsAsync(request);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(4));
        }
    }
}
