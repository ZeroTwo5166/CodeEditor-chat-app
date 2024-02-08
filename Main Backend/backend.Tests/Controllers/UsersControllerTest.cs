using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using backend.Controllers;
using Microsoft.IdentityModel.Tokens;
using Moq;
using backend.Repositories;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using backend.ModelsDTO;

namespace backend.Tests.Controllers
{
    public class UsersControllerTest
    {

        //AAA
        //Arrange => arrangement for this particular test
        //Act => Action performed in this test
        //Assert => Check output/ validate result
        [Fact]
        public async Task GetIdByUsername_ReturnsOkResult_WithUserId()
        {
            // Arrange
            var username = "testUser";
            var userId = Guid.NewGuid();
            var mockRepo = new Mock<IUserRepository>(); //create a mock object of IUserRepository => simulates the behaviour of repo
            //when the GetIdByUsername method of the repository is called with the username parameter,
            //it should return a User object with the ID set to the userId generated earlier.
            mockRepo.Setup(repo => repo.GetIdByUsername(username)).ReturnsAsync(new User { Id = userId });
            var controller = new UsersController(mockRepo.Object); //creates an instance of UsersControllers injecting the mock repo object

            // Act
            var result = await controller.GetIdByUsername(username);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(userId, ((Guid)okResult.Value.GetType().GetProperty("userId").GetValue(okResult.Value)));
        }

        [Fact]
        public async Task GetIdByUsername_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            var username = "nonExistingUser";
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetIdByUsername(username)).ReturnsAsync((User)null);
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.GetIdByUsername(username);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsOkResult_WhenLoginSuccessful()
        {
            // Arrange
            var user = new LoginDTO { Email = "test@example.com", Password = "password" };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.Login(user)).ReturnsAsync("Success");
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.Login(user);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            var user = new LoginDTO { Email = "nonExistingUser@example.com", Password = "password" };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.Login(user)).ReturnsAsync("User not found");
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.Login(user);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }


        [Fact]
        public async Task Register_ReturnsOkResult_WhenRegistrationSuccessful()
        {
            // Arrange
            var user = new User { Email = "test@example.com", Username = "testUser", Password = "password" };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.Register(user)).ReturnsAsync("Registration successful");
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.Register(user);

            // Assert
            Assert.IsType<OkObjectResult>(result); //return Ok(new {})
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenEmailAlreadyExists()
        {
            // Arrange
            var user = new User { Email = "existing@example.com", Username = "testUser", Password = "password" };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.Register(user)).ReturnsAsync("Email already exists");
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.Register(user);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result); //return BadRequest()
        }


        [Fact]
        public async Task Register_ReturnsBadRequest_WhenUsernameAlreadyExists()
        {
            // Arrange
            var user = new User { Email = "new@example.com", Username = "existingUser", Password = "password" };
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.Register(user)).ReturnsAsync("Username already exists");
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.Register(user);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetUserById_ReturnsOkResult_WithUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(new User { Id = userId });
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.GetUserById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<User>(okResult.Value);
        }

        [Fact]
        public async Task GetUserById_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockRepo = new Mock<IUserRepository>();
            mockRepo.Setup(repo => repo.GetUserById(userId)).ReturnsAsync((User)null);
            var controller = new UsersController(mockRepo.Object);

            // Act
            var result = await controller.GetUserById(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }



    }
}
