using AutoMapper;
using FluentAssertions;
using FluentAssertions.Execution;
using HouseInventory.Data.Entities;
using HouseInventory.Models.DTOs;
using HouseInventory.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace HouseInventoryTests.Services
{
    [ExcludeFromCodeCoverage]
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IMapper> _mapperMock;

        public UserServiceTests()
        {
            _mapperMock = new Mock<IMapper>();
            _userManagerMock = MockUserManager();

            _userService = new UserService(_userManagerMock.Object, _mapperMock.Object);    
        }

        #region Helper Methods

        private Mock<UserManager<User>> MockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var passwordHasher = new Mock<IPasswordHasher<User>>();
            var userValidators = new List<IUserValidator<User>> { new Mock<IUserValidator<User>>().Object };
            var passwordValidators = new List<IPasswordValidator<User>> { new Mock<IPasswordValidator<User>>().Object };
            var keyNormalizer = new Mock<ILookupNormalizer>();
            var errors = new Mock<IdentityErrorDescriber>();
            var services = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<UserManager<User>>>();

            return new Mock<UserManager<User>>(
                store.Object,
                options.Object,
                passwordHasher.Object,
                userValidators,
                passwordValidators,
                keyNormalizer.Object,
                errors.Object,
                services.Object,
                logger.Object
            );
        }

        private (User, UserLoginDto) GetDummyUserAndLoginUser()
        {
            var dummyUserLoginDto = new UserLoginDto() { Email = "test@test.com", Password = "abcd123", RememberMe = false };
            var dummyUser = new User() { Email = dummyUserLoginDto.Email, PasswordHash = GetHashedPassword(dummyUserLoginDto, dummyUserLoginDto.Password) };

            return (dummyUser, dummyUserLoginDto);
        }

        private string GetHashedPassword<T>(T user, string password) where T : class
        {
            return new PasswordHasher<T>().HashPassword(user, password);
        }

        #endregion

        #region ValidateUserCredentialsAsync Tests

        [Fact]
        public async Task ValidateUserCredentialsAsync_ShouldReturnFalseAsUserIsNotFound()
        {
            // Arrange
            _userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(default(User)).Verifiable();
            var (_, dummyUserLoginDto) = GetDummyUserAndLoginUser();

            // Act
            var result = await _userService.ValidateUserCredentialsAsync(dummyUserLoginDto);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeFalse();
                _userManagerMock.VerifyAll();
            }
        }

        [Fact]
        public async Task ValidateUserCredentialsAsync_ShouldReturnFalseAsPasswordDidNotMatch()
        {
            // Arrange
            var (dummyUser, dummyUserLoginDto) = GetDummyUserAndLoginUser();

            _userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(dummyUser).Verifiable();
            _userManagerMock.Setup(m => m.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(false);

            // Act
            var result = await _userService.ValidateUserCredentialsAsync(dummyUserLoginDto);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeFalse();
                _userManagerMock.VerifyAll();
            }
        }

        [Fact]
        public async Task ValidateUserCredentialsAsync_ShouldReturnTrue()
        {
            // Arrange
            var (dummyUser, dummyUserLoginDto) = GetDummyUserAndLoginUser();

            _userManagerMock.Setup(m => m.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(dummyUser).Verifiable();
            _userManagerMock.Setup(m => m.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var result = await _userService.ValidateUserCredentialsAsync(dummyUserLoginDto);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                _userManagerMock.VerifyAll();
            }
        }

        #endregion

        #region RegisterUserAsync Tests

        [Fact]
        public async Task RegisterUserAsync_ShouldThrowNullReferenceExceptionAsUserIsNull()
        {
            // Arrange
            var dummyUserRegistrationDto = new UserRegistrationDto
            {
                Email = "test@test.hu",
                UserName = "tesztelek",
                Password = "Password123!"
            };

            // Act
            Func<Task> result = async () => await _userService.RegisterUserAsync(dummyUserRegistrationDto);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().BeOfType<Func<Task>>();
                await result.Should().ThrowAsync<NullReferenceException>();
            }
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnSuccess()
        {
            // Arrange
            var dummyUserRegistrationDto = new UserRegistrationDto
            {
                Email = "test@test.hu",
                UserName = "tesztelek",
                Password = "Password123!"
            };

            _mapperMock.Setup(m => m.Map<User>(It.IsAny<UserRegistrationDto>()))
                .Returns((UserRegistrationDto userRegistrationDto) => new User
                {
                    Email = userRegistrationDto.Email,
                    UserName = userRegistrationDto.UserName,
                    PasswordHash = GetHashedPassword(userRegistrationDto, userRegistrationDto.Password)
                }).Verifiable();

            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Verifiable();
            _userManagerMock.Setup(m => m.AddToRolesAsync(It.IsAny<User>(), It.IsAny<IEnumerable<string>>())).ReturnsAsync(IdentityResult.Success).Verifiable();

            // Act
            var result = await _userService.RegisterUserAsync(dummyUserRegistrationDto);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().BeOfType<IdentityResult>();
                result.Should().Be(IdentityResult.Success);
                result.Succeeded.Should().BeTrue();

                _mapperMock.VerifyAll();
                _userManagerMock.VerifyAll();
            }
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnFailed()
        {
            // Arrange
            var dummyUserRegistrationDto = new UserRegistrationDto
            {
                Email = "test@test.hu",
                UserName = "tesztelek",
                Password = "Password123!"
            };

            _mapperMock.Setup(m => m.Map<User>(It.IsAny<UserRegistrationDto>()))
                .Returns((UserRegistrationDto userRegistrationDto) => new User
                {
                    Email = userRegistrationDto.Email,
                    UserName = userRegistrationDto.UserName,
                    PasswordHash = GetHashedPassword(userRegistrationDto, userRegistrationDto.Password)
                }).Verifiable();

            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(new IdentityError
            {
                Code = "TestError",
                Description = "This is a simple test error."
            })).Verifiable();

            // Act
            var result = await _userService.RegisterUserAsync(dummyUserRegistrationDto);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().BeOfType<IdentityResult>();
                result.Succeeded.Should().BeFalse();

                _mapperMock.VerifyAll();
                _userManagerMock.VerifyAll();
            }
        }

        #endregion
    }
}
