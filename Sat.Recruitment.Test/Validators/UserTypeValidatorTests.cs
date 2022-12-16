using FluentAssertions;
using Sat.Recruitment.Api.Utils.Validators;
using Xunit;

namespace Sat.Recruitment.Test.Validators
{
    public class UserTypeValidatorTests
    {
        private readonly UserTypeValidator _sut;

        public UserTypeValidatorTests()
        {
            _sut = new UserTypeValidator();
        }

        [Theory]
        [InlineData("Normal", true)]
        [InlineData("SuperUser", true)]
        [InlineData("Premium", true)]
        [InlineData("UnknownType", false)]
        [InlineData("", false)]
        public void IsValid_ExecutesSuccesfully(string userType, bool expectedResult)
        {
            // Act
            var result = _sut.IsValid(userType);

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}
