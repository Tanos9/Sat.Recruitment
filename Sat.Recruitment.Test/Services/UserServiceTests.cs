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
            var users = GetUsers();
            var userCreateRequest = new UserCreateRequest
            {
                Name = name,
                Address = address,
                Email = email,
                Money = money,
                Phone = phone,
                UserType = userType
            };

            _repository.Setup(m => m.GetAll()).Returns(Task.FromResult(users));

            // Act
            var result = _sut.CreateUser(userCreateRequest).Result;

            // Assert
            result.Should();
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
        [InlineData("goodnight moon", "moon", true)]
        [InlineData("hello world", "hi", false)]
        public void Contains(string input, string sub, bool expected)
        {
            var actual = input.Contains(sub);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetAllUsers_Successful()
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
