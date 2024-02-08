using backend.Interfaces;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : Controller
    {

        //private readonly IProjectRepository _context;
        private readonly DelegateProjectRepository _repository;

        public ProjectsController(DelegateProjectRepository repository)
        {
            _repository = repository;
        }


        //Get all projects (API)
        [HttpGet("getallprojects")]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            var allProjects = await _repository.GetAllProjectsAndUpdateTitleDel(null);

            // Check the result and return appropriate response
            if (allProjects is IEnumerable<Project> projects)
            {
                // Successfully retrieved all projects
                return Ok(projects);
            }
            else
            {
                // Failed to retrieve projects
                return BadRequest("Failed to retrieve projects.");
            }
        }


        //Get projects by userId (List)
        [HttpGet("getProjectsByUserId/{userId}")] 
        public async Task<ActionResult<IEnumerable<Project>>> GetProjectsByUserId(Guid userId)
        {
            // Retrieve projects based on the provided userId
            var projectsById = await _repository.GetProjectsByUserDel(userId);
            //var projects = await _context.GetProjectsByUserId(userId);

            if (projectsById != null && projectsById.Any())
            {
                return Ok(projectsById);

            }
            return BadRequest("Found None");
        }

        //create project
        [HttpPost("create")] 
        public async Task<ActionResult> Create([FromBody] Project project)
        {

            // Invoke the delegate to create a new project
            Guid projectId = await _repository.CreateProjectDel(project);

            // Check if the project was successfully created
            if (projectId != Guid.Empty)
            {
                return Ok(projectId);
            }
            else
            {
                return BadRequest("Failed to create project.");
            }
  
        }
 
        [HttpGet("getCodeFromProject/{projectId}")]
        public async Task<ActionResult<string>> GetCodeFromProject(Guid projectId)
        {

            //var code = await _context.GetCodeFromProject(projectId);
            var code = await _repository.GetCodeFromProjectDel(projectId);

            if(code == "Enter correct ProjectId")
            {
                return BadRequest(code);
            }
            else if(code == "Project not found")
            {
                return NotFound(code);
            }

            return Ok(new { code });

        } 

        [HttpDelete("deleteProjectById/{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            try
            {
                //var success = await _context.DeleteProject(id);
                var success = await _repository.DeleteProjectDel(id);
                if (success)
                    return Ok(new { message = "Project Deleted" });
                else
                    return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("updateProjectTitle/{projectId}/{newTitle}")]
        public async Task<IActionResult> UpdateProjectTitle(Guid projectId, string newTitle)
        {
            try
            {
                // Call the delegate to update the project title
                var result = await _repository.GetAllProjectsAndUpdateTitleDel(new Tuple<Guid, string>(projectId, newTitle));

                if (result is bool success && success)
                {
                    return Ok(new { message = "Project title updated successfully." });
                }
                else
                {
                    return NotFound("Project not found.");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }

}
