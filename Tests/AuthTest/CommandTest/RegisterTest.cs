using Application.Authorize.Commands.Register;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Moq;

namespace Tests.AuthTest.CommandTest
{
    [TestFixture]
    public class RegisterTest
    {
        private Mock<IAuthRepository> _authRepositoryMock;
        private Mock<IJwtGenerator> _jwtGeneratorMock;
        private Mock<IMapper> _mapperMock;
        private RegisterCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            // Initialize mock objects, fakes for database, JWT generation, automapper
            _authRepositoryMock = new Mock<IAuthRepository>();
            _jwtGeneratorMock = new Mock<IJwtGenerator>();
            _mapperMock = new Mock<IMapper>();

            // Create instance of RegisterCommandHandler using the mocked dependencies
            _handler = new RegisterCommandHandler(
                _authRepositoryMock.Object, // mocked database
                _jwtGeneratorMock.Object, // mocked JWT generator
                _mapperMock.Object // mocked AutoMapper
            );
        }

        [Test]
        public async Task Handle_EmailAlreadyExists_ReturnsFailure()
        {
            // This test checks that the handler returns an error if the email is already taken

            // Arrange
            var command = new RegisterCommand("TestUser", "test@example.com", "Password123");

            _authRepositoryMock
                .Setup(repository => repository.EmailExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Email is already registered."));
        }

        [Test]
        public async Task Handle_ValidRegistration_ReturnsToken()
        {
            // This test simulates a successful registration and checks that a token is returned

            // Arrange
            var command = new RegisterCommand("TestUser", "test@example.com", "Password123");
            var user = new User { UserEmail = command.UserEmail, UserName = command.UserName };

            _authRepositoryMock.Setup(repository => repository.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            _mapperMock.Setup(mapper => mapper.Map<User>(command)).Returns(user);
            _jwtGeneratorMock.Setup(jwt => jwt.GenerateToken(user)).Returns("mocked-jwt");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.EqualTo("mocked-jwt"));
            _authRepositoryMock.Verify(repository => repository.CreateUserAsync(It.Is<User>(user => user.UserEmail == command.UserEmail)), Times.Once);
            _authRepositoryMock.Verify(repository => repository.SaveChangesAsync(), Times.Once);
        }
    }
}
