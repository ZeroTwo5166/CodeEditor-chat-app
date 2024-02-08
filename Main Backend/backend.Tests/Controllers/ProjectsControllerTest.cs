using backend.Controllers;
using backend.Interfaces;
using backend.Models;
using backend.ModelsDTO;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace backend.Tests.Controllers
{
    
    public class ProjectsControllerTest
    {


        //These tests are for Project controller when not using Delegate. If you remove the dependency "DelegateProjectRepository" from ProjectController and-
        //And use the ProjectRepository with DI, this test succeds as it is targeted for that specific projectcontroller 
        /*
        [Fact]
        public async Task GetProjects_ReturnsOkResult_WithProjects()
        {
            // Arrange
            var mockRepo = new Mock<IProjectRepository>();
            var projects = new List<Project> { new Project { Id = Guid.NewGuid(), ProjectName = "Project 1" }, new Project { Id = Guid.NewGuid(), ProjectName = "Project 2" } };
            mockRepo.Setup(repo => repo.GetAllProjects()).ReturnsAsync(projects);
            var controller = new ProjectsController(mockRepo.Object);

            // Act
            var result = await controller.GetProjects();

            // Assert
            var okResult = Assert.IsAssignableFrom<ActionResult<IEnumerable<Project>>>(result);
            Assert.NotNull(okResult);
        }

        [Fact]
        public async Task GetProjectsByUserId_ReturnsOkResult_WithProjects()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var mockRepo = new Mock<IProjectRepository>();
            var projects = new List<Project> { new Project { Id = Guid.NewGuid(), ProjectName = "Project 1", UserId = userId }, new Project { Id = Guid.NewGuid(), ProjectName = "Project 2", UserId = userId } };
            mockRepo.Setup(repo => repo.GetProjectsByUserId(userId)).ReturnsAsync(projects);
            var controller = new ProjectsController(mockRepo.Object);

            // Act
            var result = await controller.GetProjectsByUserId(userId);

            // Assert
            var okResult = Assert.IsAssignableFrom<ActionResult<IEnumerable<Project>>>(result);
            Assert.NotNull(okResult);
            var objectResult = Assert.IsType<OkObjectResult>(okResult.Result);
            var returnedProjects = Assert.IsAssignableFrom<IEnumerable<Project>>(objectResult.Value);
            Assert.Equal(2, returnedProjects.Count());
        }

        [Fact]
        public async Task Create_ReturnsOkResult_WithProjectId()
        {
            // Arrange
            var mockRepo = new Mock<IProjectRepository>();
            var project = new Project { Id = Guid.NewGuid(), ProjectName = "New Project" };
            mockRepo.Setup(repo => repo.CreateProject(It.IsAny<Project>())).ReturnsAsync(project.Id);
            var controller = new ProjectsController(mockRepo.Object);

            // Act
            var result = await controller.Create(project);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(project.Id, okResult.Value.GetType().GetProperty("projectId").GetValue(okResult.Value));
        }

        [Fact]
        public async Task GetCodeFromProject_ReturnsOkResult_WithCode()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var code = "Sample code";
            var mockRepo = new Mock<IProjectRepository>();
            mockRepo.Setup(repo => repo.GetCodeFromProject(projectId)).ReturnsAsync(code);
            var controller = new ProjectsController(mockRepo.Object);

            // Act
            var result = await controller.GetCodeFromProject(projectId);

            // Assert
            //Assert.Equal() Failure
            //  ↓ (pos 0)
            //Expected: Sample code
            //Actual: { code = Sample code }
            // ↑ (pos 0)

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var responseObject = okResult.Value as dynamic;
            var extractedCode = responseObject?.ToString();
            Assert.Equal(code, extractedCode);
        }

        [Fact]
        public async Task GetCodeFromProject_ReturnsBadRequest_WhenCodeIsInvalid()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var mockRepo = new Mock<IProjectRepository>();
            var invalidCode = "Enter correct ProjectId";
            mockRepo.Setup(repo => repo.GetCodeFromProject(projectId)).ReturnsAsync(invalidCode);
            var controller = new ProjectsController(mockRepo.Object);

            // Act
            var result = await controller.GetCodeFromProject(projectId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(invalidCode, badRequestResult.Value);
        }

        [Fact]
        public async Task GetCodeFromProject_ReturnsNotFound_WhenProjectNotFound()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var mockRepo = new Mock<IProjectRepository>();
            var notFoundCode = "Project not found";
            mockRepo.Setup(repo => repo.GetCodeFromProject(projectId)).ReturnsAsync(notFoundCode);
            var controller = new ProjectsController(mockRepo.Object);

            // Act
            var result = await controller.GetCodeFromProject(projectId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(notFoundCode, notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteProject_ReturnsOkResult_WhenDeleted()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var mockRepo = new Mock<IProjectRepository>();
            mockRepo.Setup(repo => repo.DeleteProject(projectId)).ReturnsAsync(true);
            var controller = new ProjectsController(mockRepo.Object);

            // Act
            var result = await controller.DeleteProject(projectId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Project Deleted", okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value));
        }

        [Fact]
        public async Task UpdateProjectTitle_ReturnsOkResult_WhenUpdated()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var newTitle = "Updated Project Title";
            var mockRepo = new Mock<IProjectRepository>();
            mockRepo.Setup(repo => repo.UpdateProjectTitle(projectId, newTitle)).ReturnsAsync(true);
            var controller = new ProjectsController(mockRepo.Object);

            // Act
            var result = await controller.UpdateProjectTitle(projectId, newTitle);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Project title updated successfully.", okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value));
        }*/
    }

} 
