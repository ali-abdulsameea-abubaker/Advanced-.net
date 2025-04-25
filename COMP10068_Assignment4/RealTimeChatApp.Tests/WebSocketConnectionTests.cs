using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RealTimeChatApp.Hubs;
using RealTimeChatApp.Models;
using Xunit;

namespace RealTimeChatApp.Tests
{
    /// <summary>
    /// Test class for WebSocketHandler functionality
    /// </summary>
    public class WebSocketHandlerTests : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mock<IServiceProvider> _serviceProviderMock;
        private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
        private readonly Mock<IServiceScope> _serviceScopeMock;

        /// <summary>
        /// Initializes a new instance of the WebSocketHandlerTests class
        /// </summary>
        public WebSocketHandlerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new ApplicationDbContext(options);

            _serviceProviderMock = new Mock<IServiceProvider>();
            _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            _serviceScopeMock = new Mock<IServiceScope>();

            _serviceScopeFactoryMock.Setup(x => x.CreateScope()).Returns(_serviceScopeMock.Object);
            _serviceScopeMock.Setup(x => x.ServiceProvider).Returns(_serviceProviderMock.Object);
            _serviceProviderMock.Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                .Returns(_serviceScopeFactoryMock.Object);
            _serviceProviderMock.Setup(x => x.GetService(typeof(ApplicationDbContext)))
                .Returns(_dbContext);
        }

        /// <summary>
        /// Disposes the test resources
        /// </summary>
        public void Dispose()
        {
            _dbContext.Dispose();
        }

        private WebSocketHandler CreateHandler()
        {
            return new WebSocketHandler(_serviceProviderMock.Object);
        }

        private Mock<WebSocket> CreateMockWebSocket(WebSocketState state = WebSocketState.Open)
        {
            var mock = new Mock<WebSocket>();
            mock.Setup(x => x.State).Returns(state);
            mock.Setup(x => x.ReceiveAsync(It.IsAny<ArraySegment<byte>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new WebSocketException()); // Simulate immediate disconnect
            return mock;
        }

        private HttpContext CreateHttpContext(string username)
        {
            var context = new DefaultHttpContext();
            context.Request.Query = new QueryCollection(
                new System.Collections.Generic.Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
                {
                    { "username", username }
                });
            return context;
        }

        /// <summary>
        /// Tests that a valid username does not close the connection with invalid payload status
        /// </summary>
        [Fact]
        public async Task ValidUsername_ShouldNotCloseWithInvalidPayload()
        {
            // Arrange
            var handler = CreateHandler();
            var socket = CreateMockWebSocket();
            var context = CreateHttpContext("ValidUser");

            // Act
            await handler.HandleConnection(context, socket.Object);

            // Assert
            socket.Verify(x => x.CloseAsync(
                WebSocketCloseStatus.InvalidPayloadData,
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Never);
        }

        /// <summary>
        /// Tests that an invalid username closes the connection with invalid payload status
        /// </summary>
        [Fact]
        public async Task InvalidUsername_ShouldCloseWithInvalidPayload()
        {
            // Arrange
            var handler = CreateHandler();
            var socket = CreateMockWebSocket();
            var context = CreateHttpContext("Invalid123");

            // Act
            await handler.HandleConnection(context, socket.Object);

            // Assert
            socket.Verify(x => x.CloseAsync(
                WebSocketCloseStatus.InvalidPayloadData,
                "Invalid username (letters only, max 12 characters)",
                It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Tests that the connection handler sends welcome messages for new users
        /// </summary>
        [Fact]
        public async Task HandleConnection_ShouldSendWelcomeMessages()
        {
            // Arrange
            var handler = CreateHandler();
            var socket = CreateMockWebSocket();
            var context = CreateHttpContext("NewUser");

            // Act
            await handler.HandleConnection(context, socket.Object);

            // Assert
            socket.Verify(x => x.SendAsync(
                It.Is<ArraySegment<byte>>(data => Encoding.UTF8.GetString(data).Contains("NewUser joined the chat")),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None), Times.AtLeastOnce);
        }

        /// <summary>
        /// Tests that disconnection handler properly updates the database
        /// </summary>
        [Fact]
        public async Task HandleDisconnection_ShouldUpdateDatabase()
        {
            // Arrange
            var handler = CreateHandler();
            var username = "TestUser";
            var user = new User { Username = username, ConnectedTime = DateTime.UtcNow };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            // Act
            await handler.HandleDisconnection(_dbContext, username, "disconnected");

            // Assert
            var updatedUser = await _dbContext.Users.FirstAsync();
            Assert.NotNull(updatedUser.DisconnectedTime);
        }
    }
}