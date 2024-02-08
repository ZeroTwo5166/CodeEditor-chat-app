using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using backend.Models;
using Xunit;
using backend.Repositories;
using backend.ModelsDTO;


namespace backend.Tests.Repositories
{
    public class UserRepositoryTest
    {
        private DbContextOptions<ApplicationDbContext> options;
        private ApplicationDbContext context;


        public UserRepositoryTest()
        {
            //Initialize a inmemory db
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "testdb")
                .Options;

            context = new ApplicationDbContext(options);
            context.Database.EnsureDeleted(); // Make sure all the exisiting data in the memory-db is deleted
            context.Users.Add(new User { Id = Guid.Parse("6353C2738DB24A32ACAB08DC227BEBA6"), Username = "Subarna", Email = "subarnagrg23@gmail.com", Password = "test", ProfilePic = null, Projects = null });
            context.Users.Add(new User { Id = Guid.Parse("1FAD7E74-1E2E-456C-BF25-8B4AD2A1139A"), Username = "JohnDoe", Email = "johndoe@example.com", Password = "password123", ProfilePic = null, Projects = null });
            context.Users.Add(new User { Id = Guid.Parse("3F4E24EF-6D90-42DC-A25C-94A3125A1AC1"), Username = "JaneSmith", Email = "janesmith@example.com", Password = "qwerty", ProfilePic = null, Projects = null });
            context.Users.Add(new User { Id = Guid.Parse("E7FA47F4-CD8B-4084-A348-F181DEA9D588"), Username = "Alice", Email = "alice@example.com", Password = "password", ProfilePic = null, Projects = null });
            context.SaveChangesAsync();
        }


        [Fact]
        public async Task GetUserById()
        {
            //Arrange
            UserRepository userRep = new UserRepository(context);
            Guid userId = Guid.Parse("E7FA47F4-CD8B-4084-A348-F181DEA9D588");

            //Act
            var result = await userRep.GetUserById(userId);

            //Assert
            Assert.Equal(userId, result.Id);
            Assert.Equal("Alice", result.Username); //Succed
        }

        [Fact]
        public async Task GetUserById_IdDoesntExists()
        {
            //Arrange
            UserRepository userRep = new UserRepository(context);
            Guid userId = Guid.Parse("F7FA47F4-CD0B-4084-A348-F180DEA9D588");

            //Act
            var result = await userRep.GetUserById(userId);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Login_UserNotFound_ReturnsMessage()
        {
            //Arrange
            UserRepository userRep = new UserRepository(context);
            string email = "NonExistentUser";
            string password = "NonExistentPassword";

            LoginDTO loginData = new LoginDTO() { Email = email, Password = password };

            //Act
            var result = await userRep.Login(loginData);

            // Assert
            Assert.Equal("User not found", result);

        }

        [Fact]
        public async Task Login_UserFound()
        {
            //Arrange
            UserRepository userRep = new UserRepository(context);
            string email = "janesmith@example.com";
            string password = "qwerty";

            LoginDTO loginData = new LoginDTO() { Email = email, Password = password };

            //Act
            var result = await userRep.Login(loginData);

            // Assert
            Assert.Equal("Success", result);
        }

        [Fact]
        public async Task Register_NewUser_SuccessfullyRegistered()
        {
            // Arrange
            var userRepository = new UserRepository(context);

            var newUser = new User { Email = "newuser@example.com", Username = "newuser", Password = "password" };

            // Act
            var result = await userRepository.Register(newUser);

            // Assert
            Assert.Equal("Registration successful", result);

        }

        [Fact]
        public async Task Register_NewUser_EmailAlreadyExists()
        {
            // Arrange
            var userRepository = new UserRepository(context);

            var newUser = new User { Email = "johndoe@example.com", Username = "newww", Password = "password" };

            // Act
            var result = await userRepository.Register(newUser);

            // Assert
            Assert.Equal("Email already exists", result);

        }

        [Fact]
        public async Task GetProfilePic_NotExists()
        {
            // Arrange
            var userRepository = new UserRepository(context);

            RetrieveImageDTO username =  new RetrieveImageDTO() { Username = "Subarna" };

            // Act
            var result = await userRepository.GetProfilePic(username);

            // Assert
            Assert.Equal("Profilepic not found", result);
        }


        [Fact]
        public async Task GetUserByUsername_Success()
        {
            // Arrange
            var userRepository = new UserRepository(context);

            string username = "JohnDoe";

            // Act
            var result = await userRepository.GetIdByUsername(username);

            var expectedUser = new User { Id = Guid.Parse("1FAD7E74-1E2E-456C-BF25-8B4AD2A1139A"), Username = "JohnDoe", Email = "johndoe@example.com", Password = "password123", ProfilePic = null, Projects = null };

            // Assert
            //Assert.Equal(expectedUser, result); //failed? 
            Assert.Equal(expectedUser.Username, result.Username);
            Assert.Equal(expectedUser.Id, result.Id);
            Assert.Equal(expectedUser.Email, result.Email);
            Assert.Equal(expectedUser.Password, result.Password);

        }
    }
}
