using backend.Models;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace backend.Tests.Repositories
{
    public class ProjectRepositoryTest
    {
        private DbContextOptions<ApplicationDbContext> options;
        private ApplicationDbContext context;


        public ProjectRepositoryTest()
        {
            //Initialize a inmemory db
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "testdb")
                .Options;

            context = new ApplicationDbContext(options);
            context.Database.EnsureDeleted(); // Make sure all the exisiting data in the memory-db is deleted
            context.Projects.Add(new Project { Id = Guid.Parse("3F4E24EF-6D90-42DC-A25C-94A3125A1AC1"), ProjectName = "Python", UserId = Guid.Parse("1fad7e74-1e2e-456c-bf25-8b4ad2a1139a"), Code = "python-code", User = null });
            context.Projects.Add(new Project { Id = Guid.Parse("E7FA47F4-CD8B-4084-A348-F181DEA9D588"), ProjectName = "Java", UserId = Guid.Parse("1fad7e74-1e2e-456c-bf25-8b4ad2a1139a"), Code = "java-code", User = null });
            context.Projects.Add(new Project { Id = Guid.Parse("A785F7F4-9B5A-4DA1-B13A-45BC9F5C8D72"), ProjectName = "C#", UserId = Guid.Parse("1fad7e74-1e2e-456c-bf25-8b4ad2a1139a"), Code = "csharp-code", User = null });
            context.SaveChangesAsync();
        }

        [Fact]
        public async Task CreateProject_AlreadyExistsInDatabase()
        {
            // Arrange
            var repository = new ProjectRepository(context);
            var project = new Project { Id = Guid.NewGuid(), ProjectName = "New Project", UserId = Guid.NewGuid(), Code = "new-code" };

            // Act
            var projectId = await repository.CreateProject(project);
            var addedProject = await context.Projects.FindAsync(projectId);

            // Assert
            Assert.NotNull(addedProject);
            Assert.Equal(projectId, addedProject.Id);
        }

        [Fact]
        public async Task DeleteProject_ShouldRemoveProjectFromDatabase()
        {
            // Arrange
            var repository = new ProjectRepository(context);
            var projectId = Guid.Parse("3F4E24EF-6D90-42DC-A25C-94A3125A1AC1");

            // Act
            var isDeleted = await repository.DeleteProject(projectId);
            var deletedProject = await context.Projects.FindAsync(projectId);

            // Assert
            Assert.True(isDeleted);
            Assert.Null(deletedProject);
        }

        [Fact]
        public async Task GetAllProjects_ShouldReturnAllProjects()
        {
            // Arrange
            var repository = new ProjectRepository(context);

            // Act
            var projects = await repository.GetAllProjects();
            var projectList = projects.ToList(); // Convert to a list to access elements by index

            // Assert
            Assert.Equal(3, projects.Count());
            Assert.Equal("Java", projectList[1].ProjectName);
        }

        [Fact]
        public async Task GetCodeFromProject_WithValidProjectId_ShouldReturnCode()
        {
            // Arrange
            var repository = new ProjectRepository(context);
            var projectId = Guid.Parse("3F4E24EF-6D90-42DC-A25C-94A3125A1AC1");

            // Act
            var code = await repository.GetCodeFromProject(projectId);

            // Assert
            Assert.Equal("python-code", code);
        }

        [Fact]
        public async Task GetCodeFromProject_WithInvalidProjectId_ShouldReturnErrorMessage()
        {
            // Arrange
            var repository = new ProjectRepository(context);
            var projectId = Guid.NewGuid(); // Non-existing project ID

            // Act
            var result = await repository.GetCodeFromProject(projectId);

            // Assert
            Assert.Equal("Project not found", result);
        }

        [Fact]
        public async Task GetProjectById_WithValidProjectId_ShouldReturnProject()
        {
            // Arrange
            var repository = new ProjectRepository(context);
            var projectId = Guid.Parse("3F4E24EF-6D90-42DC-A25C-94A3125A1AC1");

            // Act
            var project = await repository.GetProjectById(projectId);

            // Assert
            Assert.NotNull(project);
            Assert.Equal(projectId, project.Id);
        }

        [Fact]
        public async Task GetProjectsByUserId_ShouldReturnProjectsBelongingToUserId()
        {
            // Arrange
            var repository = new ProjectRepository(context);
            var userId = Guid.NewGuid(); // Random user ID, not from existing projects

            // Act
            var projects = await repository.GetProjectsByUserId(userId);

            // Assert
            Assert.Empty(projects);
        }

        [Fact]
        public async Task UpdateProjectTitle_WithValidProjectId_ShouldUpdateProjectTitle()
        {
            // Arrange
            var repository = new ProjectRepository(context);
            var projectId = Guid.Parse("3F4E24EF-6D90-42DC-A25C-94A3125A1AC1");
            var newTitle = "New Title";

            // Act
            var isUpdated = await repository.UpdateProjectTitle(projectId, newTitle);
            var updatedProject = await context.Projects.FindAsync(projectId);

            // Assert
            Assert.True(isUpdated);
            Assert.Equal(newTitle, updatedProject.ProjectName);
        }

        [Fact]
        public async Task UpdateProjectTitle_WithInvalidProjectId_ShouldReturnFalse()
        {
            // Arrange
            var repository = new ProjectRepository(context);
            var projectId = Guid.NewGuid(); // Non-existing project ID
            var newTitle = "New Title";

            // Act
            var isUpdated = await repository.UpdateProjectTitle(projectId, newTitle);

            // Assert
            Assert.False(isUpdated);
        }


    }
}
