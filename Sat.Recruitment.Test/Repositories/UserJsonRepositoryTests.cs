using FluentAssertions;
using Moq;
using Sat.Recruitment.Api.DataAccess;
using Sat.Recruitment.Api.Models.Enums;
using Sat.Recruitment.Api.Models.User;
using Sat.Recruitment.Api.Repositories.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Sat.Recruitment.Test.Repositories
{
    public class UserJsonRepositoryTests
    {
        private readonly UserJsonRepository _sut;
        private readonly Mock<IFileManagerJson> _fileManagerJson = new Mock<IFileManagerJson>();

        public UserJsonRepositoryTests()
        {
            _sut = new UserJsonRepository(_fileManagerJson.Object);
        }

        [Fact]
        public async void GetAll_ExecutesSuccesfully()
        {
           // Arrange
           var users = GetUsers();
           var jsonFile = GetJsonStream();

            _fileManagerJson.Setup(f => f.ReadJsonFile()).ReturnsAsync(jsonFile);

           // Act
           var result = await _sut.GetAll();

           // Assert
            result.Should().NotBeEmpty()
                .And.BeEquivalentTo(users);
        }

        [Theory]
        [InlineData("Juan", "Peru 2464", "Juan@marmol.com", "+5491154762319", true)]
        [InlineData("Mike Patton", "Bungle 966", "faithnomo@gmail.com", "+5488154762333", false)]
        [InlineData("Juan", "Bungle 966", "differentJuan@marmol.com", "+5488154768998", false)]
        [InlineData("Same Mail Juan", "Ohio 487", "Juan@marmol.com", "+5491154111222", true)]
        [InlineData("Same Phone Juan", "Hawaii 811", "anotherJuan@marmol.com", "+5491154762319", true)]
        [InlineData("Juan", "Peru 2464", "sameNameAndAddress@test.com", "+9991154765555", true)]
        public async void AddMoneyGiftByUserType_ObtainsExpectedResult(string name, string address, string email, string phone, bool expectedResult)
        {
            // Arrange
            var user = new User()
            {
                Name = name,
                Address = address,
                Email = email,
                Phone = phone,
            };

            var jsonFile = GetJsonStream();

            _fileManagerJson.Setup(f => f.ReadJsonFile()).ReturnsAsync(jsonFile);

            // Act
            var result = await _sut.UserExists(user);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public async void Insert_ExecutesSuccesfully()
        {
            // Arrange
            var user = GetUsers().First();
            var jsonFile = GetJsonStream();

            _fileManagerJson.Setup(f => f.ReadJsonFile()).ReturnsAsync(jsonFile);

            // Act
            await _sut.Insert(user);
        }

        private FileStream GetJsonStream()
        {
            var path = "./Files/Users_Test.json";
            return new FileStream(path, FileMode.Open);
        }

        private IEnumerable<User> GetUsers()
        {
            return new List<User>()
            {
                new User()
                {
                    Name = "Juan",
                    Address = "Peru 2464",
                    Email = "Juan@marmol.com",
                    Money = 1234,
                    Phone = "+5491154762319",
                    UserType = UserType.Normal.ToString()
                },
                new User()
                {
                    Name = "Franco",
                    Address = "Alvear y Colombres",
                    Email = "ranco.Perez@gmail.com",
                    Money = 112234,
                    Phone = "+534645213542",
                    UserType = UserType.Premium.ToString()
                }
            };
        }
    }
}
