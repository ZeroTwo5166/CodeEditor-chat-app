using backend.Hub;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : Controller, IProjectController
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("getallprojects")] //From db
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            var allProjects = await _context.Projects.ToListAsync();

            if(allProjects !=  null && allProjects.Any())
            {
                return Ok(new
                {
                    projects = allProjects
                });
            }
            return BadRequest("Found None");
        }


        [HttpGet("getProjectsByUserId/{userId}")] //from db
        public ActionResult<IEnumerable<Project>> GetProjectsByUserId(Guid userId)
        {
            // Retrieve projects based on the provided userId
            var projects = _context.Projects.Where(p => p.UserId == userId).ToList();

            if (projects == null || projects.Count == 0)
            {
                return NotFound("No projects found for the specified user.");
            }

            return Ok(projects);
        }

  
        [HttpPost("create")] //from db
        public async Task<ActionResult> Create([FromBody]Project project)
        {
            try
            {
                _context.Projects.Add(project);

                await _context.SaveChangesAsync();
                return Ok( new { message = "Created", projectId = project.Id });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error"); // Handle the exception and return an appropriate response
            }
        }

        [HttpGet("getCodeFromProject/{projectId}")]
        public async Task<ActionResult<string>> GetCodeFromProject(Guid projectId)
        {
            try
            {
                // Retrieve the project based on the provided projectId
                var project = await _context.Projects.FindAsync(projectId);

                if (project == null)
                {
                    return NotFound("Project not found.");
                }

                // Assuming 'Code' is the property where the code is stored in the Project entity
                string code = project.Code;

                return Ok(new { code });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("deleteProjectById/{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return Ok( new { message = "Project Deleted"});
        }

        [HttpPut("updateProjectTitle/{projectId}/{newTitle}")]
        public async Task<IActionResult> UpdateProjectTitle(Guid projectId, string newTitle)
        {
            try
            {
                var project = await _context.Projects.FindAsync(projectId);

                if (project == null)
                {
                    return NotFound("Project not found.");
                }

                project.ProjectName = newTitle; // Assuming the property to update is 'Title'

                _context.Projects.Update(project);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Project title updated successfully." });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }



        //[HttpPost("update")]
        // public async Task<IActionResult> Update([FromBody])
    }
}
