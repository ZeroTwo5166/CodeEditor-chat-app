using backend.Controllers;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace backend.tests.Controllers
{
    public class ProjectControllerTest
    {
        
        [Fact]
        public async Task GetProjectsByUserId_ReturnsProjects_WhenProjectsExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            var userId = Guid.NewGuid();

            using (var context = new ApplicationDbContext(options))
            {
                // Add some test projects with a specific user ID to the in-memory database
                var projects = new List<Project>
                {
                    new Project { Id = Guid.NewGuid(), ProjectName = "Project 1", UserId = userId },
                    new Project { Id = Guid.NewGuid(), ProjectName = "Project 2", UserId = userId }
                };
                context.Projects.AddRange(projects);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                // Create the ProjectController with the in-memory database context
                var controller = new ProjectController(context);

                // Act: Retrieve projects by user ID
                var result = controller.GetProjectsByUserId(userId);

                // Assert: Verify that the result is an OkObjectResult containing the projects
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var responseData = Assert.IsType<List<Project>>(okResult.Value);
                Assert.NotEmpty(responseData);
            }
        }

        [Fact]
        public async Task GetProjects_ReturnsBadRequest_WhenNoProjectsExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                // No projects added to the database
            }

            using (var context = new ApplicationDbContext(options))
            {
                // Create the ProjectController with the in-memory database context
                var controller = new ProjectController(context);

                // Act: Retrieve all projects
                var result = await controller.GetProjects();

                // Assert: Verify that the result is BadRequest
                var actionResult = Assert.IsType<ActionResult<IEnumerable<Project>>>(result);
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
                Assert.Equal("Found None", badRequestResult.Value);
            }
        }


        [Fact]
        public async Task GetProjectsByUserId_ReturnsProjects_WhenProjectsExistForUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            var userId = Guid.NewGuid();

            using (var context = new ApplicationDbContext(options))
            {
                // Add some test projects to the in-memory database for the specific user
                var projects = new List<Project>
        {
            new Project { Id = Guid.NewGuid(), ProjectName = "Project 1", UserId = userId },
            new Project { Id = Guid.NewGuid(), ProjectName = "Project 2", UserId = userId }
        };
                context.Projects.AddRange(projects);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                // Create the ProjectController with the in-memory database context
                var controller = new ProjectController(context);

                // Act: Retrieve projects for the specific user
                var result = controller.GetProjectsByUserId(userId);

                // Assert: Verify that the result is not null and contains data
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var responseData = Assert.IsAssignableFrom<IEnumerable<Project>>(okResult.Value);
                Assert.NotEmpty(responseData);
            }
        }

        [Fact]
        public async Task GetProjectsByUserId_ReturnsNotFound_WhenNoProjectsExistForUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            var userId = Guid.NewGuid();

            using (var context = new ApplicationDbContext(options))
            {
                // No projects added to the in-memory database for the specific user
            }

            using (var context = new ApplicationDbContext(options))
            {
                // Create the ProjectController with the in-memory database context
                var controller = new ProjectController(context);

                // Act: Retrieve projects for the specific user
                var result = controller.GetProjectsByUserId(userId);

                // Assert: Verify that the result is NotFound
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
                Assert.Equal("No projects found for the specified user.", notFoundResult.Value);
            }
        }

        [Fact]
        public async Task Create_ReturnsInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            var invalidProject = new Project { Id = Guid.NewGuid() }; // Missing required properties

            using (var context = new ApplicationDbContext(options))
            {
                // Create the ProjectController with the in-memory database context
                var controller = new ProjectController(context);

                // Act: Attempt to create an invalid project
                var result = await controller.Create(invalidProject);

                // Assert: Verify that the result is an ObjectResult with status code 500 (InternalServerError)
                var objectResult = Assert.IsType<ObjectResult>(result);
                Assert.Equal(500, objectResult.StatusCode);
            }
        }


        [Fact]
        public async Task DeleteProject_ReturnsNotFound_WhenProjectDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            var projectId = Guid.NewGuid();

            using (var context = new ApplicationDbContext(options))
            {
                // No projects added to the database
            }

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new ProjectController(context);

                // Act
                var result = await controller.DeleteProject(projectId);

                // Assert
                var notFoundResult = Assert.IsType<NotFoundResult>(result);
            }
        }


        [Fact]
        public async Task UpdateProjectTitle_ReturnsNotFound_WhenProjectDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test_database")
                .Options;

            var projectId = Guid.NewGuid();
            var newTitle = "Updated Title";

            using (var context = new ApplicationDbContext(options))
            {
                // No projects added to the database
            }

            using (var context = new ApplicationDbContext(options))
            {
                var controller = new ProjectController(context);

                // Act
                var result = await controller.UpdateProjectTitle(projectId, newTitle);

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.Equal("Project not found.", notFoundResult.Value);
            }
        }

    }

}
