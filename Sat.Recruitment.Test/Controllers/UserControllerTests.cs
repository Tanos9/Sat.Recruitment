using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.Api.Models.Enums;
using Sat.Recruitment.Api.Models.User;
using Sat.Recruitment.Api.Services.Interfaces;
using System.Collections.Generic;
using Xunit;

namespace Sat.Recruitment.Test.Controllers
{
    public class UserControllerTests
    {
        private readonly UserController _sut;
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();

        public UserControllerTests()
        {
            _sut = new UserController(_userService.Object);
        }

        [Fact]
        public async void GetAllUsers_ExecutesSuccesfully()
        {
            // Arrange
            var users = GetUsers();

            _userService.Setup(m => m.GetAllUsers()).ReturnsAsync(users);

            // Act
            var result = await _sut.GetAllUsers();

            var okResult = result as OkObjectResult;
            var resultValue = (IEnumerable<User>)okResult.Value;

            // Assert
            resultValue.Should().NotBeEmpty()
                .And.BeEquivalentTo(users);
        }

        [Fact]
        public async void CreateUser_ExecutesSuccesfully()
        {
            // Arrange
            var expectedResult = new Result()
            {
                IsSuccess = true,
                Errors = "The user has been created."
            };

            var user = new UserCreateRequest()
            {
                Name = "Name",
                Address = "Address",
                Email = "email@gmail.com",
                Money = 500,
                Phone = "3535690785",
                UserType = UserType.Normal.ToString()
            };

            _userService.Setup(m => m.CreateUser(user)).ReturnsAsync(expectedResult);

            // Act
            var result = await _sut.CreateUser(user);

            var okResult = result as OkObjectResult;
            var resultValue = (Result)okResult.Value;

            // Assert
            resultValue.Should().BeEquivalentTo(expectedResult);
        }

        private IEnumerable<User> GetUsers()
        {
            return new List<User>()
            {
                new User()
                {
                    Name = "Name",
                    Address = "Address",
                    Email = "email@gmail.com",
                    Money = 500,
                    Phone = "3535690785",
                    UserType = UserType.Normal.ToString()
                },
                new User()
                {
                    Name = "Name 2",
                    Address = "Address 2",
                    Email = "email@gmail.com",
                    Money = 100,
                    Phone = "3535140890",
                    UserType = UserType.Premium.ToString()
                }
            };
        }
    }
}
