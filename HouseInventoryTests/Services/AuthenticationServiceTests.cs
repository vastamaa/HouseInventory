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
    public class AuthenticationServiceTests
    {
        private readonly AuthenticationService _authenticationService;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<SignInManager<User>> _signInManagerMock;

        public AuthenticationServiceTests()
        {
            _mapperMock = new Mock<IMapper>();
            _userManagerMock = MockUserManager();
            _signInManagerMock = MockSignInManager(_userManagerMock);
            _authenticationService = new AuthenticationService(_mapperMock.Object, _userManagerMock.Object, _signInManagerMock.Object);
        }

        public Mock<UserManager<User>> MockUserManager()
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

        public Mock<SignInManager<User>> MockSignInManager(Mock<UserManager<User>> userManager)
        {
            var contextAccessorMock = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            return new Mock<SignInManager<User>>(
                userManager.Object,
                contextAccessorMock.Object,
                claimsFactory.Object,
                null, // AuthenticationSchemeProvider
                null, // IUserConfirmation<User>
                null, // ILogger<SignInManager<User>>
                null // IOptions<IdentityOptions>
            );
        }

        [Fact]
        public async Task RegisterUserAsync_Should_Return_SucceededResult()
        {
            // Arrange
            var userRegistrationDto = new UserRegistrationDto
            {
                Email = "test@example.com",
                Password = "Password123!",
            };

            var user = new User
            {
                UserName = "test@example.com",
                Email = "test@example.com"
            };

            _mapperMock
                .Setup(m => m.Map<User>(It.IsAny<UserRegistrationDto>()))
                .Returns(user);

            _userManagerMock
                .Setup(um => um.CreateAsync(user, userRegistrationDto.Password))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock
                .Setup(um => um.AddToRolesAsync(user, userRegistrationDto.Roles))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authenticationService.RegisterUserAsync(userRegistrationDto);

            // Assert
            using (new AssertionScope())
            {
                result.Succeeded.Should().BeTrue();
                _mapperMock.VerifyAll();
                _userManagerMock.VerifyAll();
                _userManagerMock.VerifyAll();
            }
        }

        [Fact]
        public async Task RegisterUserAsync_Should_Return_FailedResult()
        {
            // Arrange
            var userRegistrationDto = new UserRegistrationDto
            {
                Email = "test@example.com",
                Password = "Password123!",
            };

            var user = new User { UserName = "test@example.com", Email = "test@example.com" };

            _mapperMock
                .Setup(m => m.Map<User>(It.IsAny<UserRegistrationDto>()))
                .Returns(user);

            var identityErrors = new List<IdentityError>
            {
                new IdentityError { Description = "Password too weak." }
            };

            var failedResult = IdentityResult.Failed(identityErrors.ToArray());

            _userManagerMock
                .Setup(um => um.CreateAsync(user, userRegistrationDto.Password))
                .ReturnsAsync(failedResult);

            // Act
            var result = await _authenticationService.RegisterUserAsync(userRegistrationDto);

            // Assert
            using (new AssertionScope())
            {
                result.Succeeded.Should().BeFalse();
                result.Errors.Should().Contain(e => e.Description == "Password too weak.");
                _mapperMock.VerifyAll();
                _userManagerMock.VerifyAll();
                _userManagerMock.VerifyAll();
            }

        }

        [Fact]
        public async Task LoginUserAsync_Should_Return_SucceededSignInResult()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Email = "test@example.com",
                Password = "Password123!",
                RememberMe = true
            };

            var user = new User
            {
                UserName = "test@example.com",
                Email = "test@example.com"
            };

            _userManagerMock
                .Setup(m => m.FindByEmailAsync(userLoginDto.Email))
                .ReturnsAsync(user)
                .Verifiable();

            _signInManagerMock
                .Setup(m => m.PasswordSignInAsync(user.UserName, userLoginDto.Password, userLoginDto.RememberMe, false))
                .ReturnsAsync(SignInResult.Success);

            // Act
            var result = await _authenticationService.LoginUserAsync(userLoginDto);

            // Assert
            using (new AssertionScope())
            {
                result.Succeeded.Should().BeTrue();
                _userManagerMock.VerifyAll();
                _signInManagerMock.VerifyAll();
            }
        }

        [Fact]
        public async Task LoginUserAsync_InvalidPassword_Should_Return_FailedSignInResult()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Email = "test@example.com",
                Password = "Password123!",
                RememberMe = true
            };

            var user = new User
            {
                UserName = "test@example.com",
                Email = "test@example.com"
            };

            _userManagerMock
                .Setup(m => m.FindByEmailAsync(userLoginDto.Email))
                .ReturnsAsync(user)
                .Verifiable();

            _signInManagerMock
                .Setup(m => m.PasswordSignInAsync(user.UserName, userLoginDto.Password, userLoginDto.RememberMe, false))
                .ReturnsAsync(SignInResult.Failed);

            // Act
            var result = await _authenticationService.LoginUserAsync(userLoginDto);

            // Assert
            using (new AssertionScope())
            {
                result.Succeeded.Should().BeFalse();
                _userManagerMock.VerifyAll();
                _signInManagerMock.VerifyAll();
            }
        }

        [Fact]
        public async Task LoginUserAsync_UserNotFound_Return_FailedSignInResult()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Email = "nonexistent@example.com",
                Password = "Password123!",
                RememberMe = false
            };

            _userManagerMock
                .Setup(um => um.FindByEmailAsync(userLoginDto.Email))
                .ReturnsAsync((User)null);

            // Act
            var result = async () => await _authenticationService.LoginUserAsync(userLoginDto);

            // Assert
            using (new AssertionScope())
            {
                await result.Should().ThrowAsync<NullReferenceException>().WithMessage("Object reference not set to an instance of an object.");
                _userManagerMock.VerifyAll();
                _signInManagerMock.VerifyAll();
            }
        }

        [Fact]
        public async Task LogoutUserAsync_Should_Successfully_Call_LogoutUserAsync()
        {
            // Arrange
            _signInManagerMock
                .Setup(m => m.SignOutAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();

            // Act
            await _authenticationService.LogoutUserAsync();

            // Assert
            using (new AssertionScope())
            {
                _signInManagerMock.VerifyAll();
            }
        }
    }
}
