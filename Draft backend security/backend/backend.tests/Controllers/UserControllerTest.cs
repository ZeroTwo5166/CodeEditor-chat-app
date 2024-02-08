using backend.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using backend.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.Query;

namespace backend.tests.Controllers
{
    public class UserControllerTest
    {
        /*
        [Fact]
        public async Task Register_ValidUser_ReturnsOkResult()
        {
            // Arrange
            var dbContextMock = new Mock<ApplicationDbContext>();
            dbContextMock.Setup(mock => mock.Users.AnyAsync(u => u.Email == "test@example.com", default)).ReturnsAsync(false);
            var controller = new UserController(dbContextMock.Object);
            var userDto = new UserDto
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "password"
            };

            // Act
            var result = await controller.Register(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Registration successful", okResult.Value);
        }
        */


        /*
        [Fact]
        public async Task Register_ReturnsBadRequest_WhenEmailAlreadyExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var existingUser = new User { Username = "existingUser", Email = "existing@example.com", Password = "existingPassword" };
                context.Users.Add(existingUser);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new UserController(context);
                var newUser = new User { Username = "existingUser", Email = "existing@example.com", Password = "newPassword" };

                // Act
                var result = await controller.Register(newUser);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Email already exists", badRequestResult.Value);
            }
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenUsernameAlreadyExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var existingUser = new User { Username = "existingUser", Email = "existing@example.com", Password = "existingPassword" };
                context.Users.Add(existingUser);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new UserController(context);
                var newUser = new User { Username = "existingUser", Email = "new@example.com", Password = "newPassword" };

                // Act
                var result = await controller.Register(newUser);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("Username already exists", badRequestResult.Value);
            }
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenCredentialsAreCorrect()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var existingUser = new User { Username = "existingUser", Email = "existing@example.com", Password = "existingPassword" };
                context.Users.Add(existingUser);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new UserController(context);
                var loginCredentials = new Login { Email = "existing@example.com", Password = "existingPassword" };

                // Act
                var result = await controller.Login(loginCredentials);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                // Additional assertions if necessary
            }
        }

        [Fact]
        public async Task GetIdByUsername_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                // No users added to the database
            }

            using (var context = new ApplicationDbContext(options))
            {
                // Create the UserController with the in-memory database context
                var controller = new UserController(context);

                // Act: Try to get the ID for a non-existing user
                var result = await controller.GetIdByUsername("non_existing_username");

                // Assert: Verify that the result is a NotFoundObjectResult
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                // Additional assertions if necessary
            }
        }

        [Fact]
        public async Task Login_ReturnsBadRequest_WhenUserDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                // No users added to the database
            }

            using (var context = new ApplicationDbContext(options))
            {
                // Create the UserController with the in-memory database context
                var controller = new UserController(context);

                // Act: Try to login with credentials of a non-existing user
                var result = await controller.Login(new Login { Email = "non_existing@example.com", Password = "password" });

                // Assert: Verify that the result is a BadRequestObjectResult
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                // Additional assertions if necessary
            }
        }

        [Fact]
        public async Task Retrieve_ReturnsBadRequest_WhenUserDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                // No users added to the database
            }

            using (var context = new ApplicationDbContext(options))
            {
                // Create the UserController with the in-memory database context
                var controller = new UserController(context);

                // Act: Try to retrieve profile picture for a non-existing user
                var result = await controller.GetProfilePic(new RetrieveImage { Username = "non_existing_user" });

                // Assert: Verify that the result is a BadRequestObjectResult
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                // Additional assertions if necessary
            }
        }
        */
    }
    

}
