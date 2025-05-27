using Application.Authorize.Queries.Login;
using Application.Interfaces;
using Domain.Models;
using Moq;

namespace Tests.AuthTest.QueryTest
{
    [TestFixture]
    public class LoginTest
    {
        private Mock<IAuthRepository> _authRepositoryMock;
        private Mock<IJwtGenerator> _jwtGeneratorMock;
        private LoginQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            // Initialize mock objects, fakes for database, JWT generation
            _authRepositoryMock = new Mock<IAuthRepository>();
            _jwtGeneratorMock = new Mock<IJwtGenerator>();

            // Create instance of LoginQueryHandler using the mocked dependencies
            _handler = new LoginQueryHandler(
                _authRepositoryMock.Object, // mocked database
                _jwtGeneratorMock.Object // mocked JWT generator
            );
        }

        [Test]
        public async Task Handle_InvalidCredentials_ReturnsFailure()
        {
            // Arrange
            var command = new LoginQuery("FakeUser", "WrongPassword");

            // Simulate user not found (or return a user with wrong hash if testing password failure)
            _authRepositoryMock.Setup(r => r.GetUserByUsernameAsync(command.UserName))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Invalid credentials."));
        }

        [Test]
        public async Task Handle_ValidCredentials_ReturnsToken() 
        {
            // Arrange
            var command = new LoginQuery("TestUser", "CorrectPassword");

            // Simulate user from DB with correct password hash
            var user = new User
            {
                UserName = "Testuser",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("CorrectPassword")
            };

            _authRepositoryMock.Setup(repository => repository.GetUserByUsernameAsync(command.UserName))
                .ReturnsAsync(user);

            _jwtGeneratorMock.Setup(jwt => jwt.GenerateToken(user)).Returns("mocked-jwt-token");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Data, Is.EqualTo("mocked-jwt-token"));
        }

        [Test]
        public async Task Handle_ThrowsException_ReturnsFailure()
        {
            // Arrange
            var command = new LoginQuery("TestUser", "Password123");

            _authRepositoryMock.Setup(r => r.GetUserByUsernameAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Database exploded"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.ErrorMessage, Does.Contain("Error during login"));
        }
    }
}
