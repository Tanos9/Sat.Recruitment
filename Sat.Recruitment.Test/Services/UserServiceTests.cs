using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Sat.Recruitment.Api.DataAccess.Interfaces;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.Api.Models.Enums;
using Sat.Recruitment.Api.Models.User;
using Sat.Recruitment.Api.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sat.Recruitment.Test.Services
{
    public class UserServiceTests
    {
        private readonly UserService _sut;
        private readonly Mock<IUserJsonRepository> _repository = new Mock<IUserJsonRepository>();
        private readonly Mock<ILogger<UserService>> _logger = new Mock<ILogger<UserService>>();
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();

        public UserServiceTests()
        {
            _sut = new UserService(_mapper.Object, _repository.Object, _logger.Object);
        }

        [Theory]
        [InlineData("Mike Mangini", "Columbus 261", "mikedrums@gmail.com", 500, "+99252456", "Premium")]
        public void CreateUser_ExecutesSuccessfully(string name, string address, string email, decimal money, string phone, string userType)
        {
            // Arrange
            var expectedResult = new Result()
            {
                IsSuccess = true,
                Errors = "The user has been created."
            };

            var users = GetUsers();

            var user = new User()
            {
                Name = name,
                Address = address,
                Email = email,
                Money = money,
                Phone = phone,
                UserType = userType
            };

            var userCreateRequest = new UserCreateRequest
            {
                Name = name,
                Address = address,
                Email = email,
                Money = money,
                Phone = phone,
                UserType = userType
            };

            _repository.Setup(m => m.GetAll()).ReturnsAsync(users);

            _mapper.Setup(x => x.Map<User>(It.IsAny<UserCreateRequest>()))
                .Returns(user);

            // Act
            var result = _sut.CreateUser(userCreateRequest).Result;

            // Assert
            _repository.Verify(m => m.Insert(It.IsAny<User>()), Times.Once);
            result.Should().BeOfType<Result>();
            result.IsSuccess.Should().Be(expectedResult.IsSuccess);
            result.Errors.Should().Be(expectedResult.Errors);
        }
        
        [Theory]
        [InlineData("Mike Mangini", "Columbus 261", "mikedrums@gmail.com", 500, "+99252456", "Premium")]
        public void CreateUser_IfUserAlreadyExists_ReturnsErrorResult(string name, string address, string email, decimal money, string phone, string userType)
        {
            // Arrange
            var expectedResult = new Result()
            {
                IsSuccess = false,
                Errors = "The user already exists."
            };

            var userCreateRequest = new UserCreateRequest
            {
                Name = name,
                Address = address,
                Email = email,
                Money = money,
                Phone = phone,
                UserType = userType
            };

            _repository.Setup(m => m.UserExists(It.IsAny<User>())).ReturnsAsync(true);

            // Act
            var result = _sut.CreateUser(userCreateRequest).Result;

            // Assert
            result.Should().BeOfType<Result>();
            result.IsSuccess.Should().Be(expectedResult.IsSuccess);
            result.Errors.Should().Be(expectedResult.Errors);
        }

        [Theory]
        [InlineData("Jens Johansson", 500, "Normal", 560)]
        [InlineData("Jordan Rudess", 80, "Normal", 144)]
        [InlineData("Janne Wirman", 5, "Normal", 5)]
        [InlineData("Alexi Laiho", 110, "SuperUser", 132)]
        [InlineData("Julia Belcasino", 80, "SuperUser", 80)]
        [InlineData("Sandra Amazon", 99, "Premium", 99)]
        [InlineData("John Petrucci", 700, "Premium", 2100)]
        public void AddMoneyGiftByUserType_ObtainsExpectedResult(string name, decimal money, string userType, decimal expectedResult)
        {
            // Arrange
            var user = new User()
            {
                Name = name,
                Money = money,
                UserType = userType
            };

            // Act
            _sut.AddMoneyGiftByUserType(user);

            // Assert
            user.Money.Should().Be(expectedResult);
        }

        [Fact]
        public void GetAllUsers_ExecutesSuccesfully()
        {
            // Arrange
            var users = GetUsers();

            _repository.Setup(m => m.GetAll()).ReturnsAsync(users);

            // Act
            var result = _sut.GetAllUsers().Result;

            // Assert
            result.Should().NotBeEmpty()
                .And.HaveCount(users.Count());
        }

        private IEnumerable<User> GetUsers()
        {
            return new List<User>()
            {
                new User()
                {
                    Name = "Name",
                    Address = "Address",
                    Email = "email",
                    Money = 500,
                    Phone = "3535690",
                    UserType = UserType.Normal.ToString()
                }
            };
        }
    }
}
