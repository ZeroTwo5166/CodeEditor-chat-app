using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Interfaces
{
    public interface IProjectController
    {
        public Task<ActionResult<IEnumerable<Project>>> GetProjects();
        public ActionResult<IEnumerable<Project>> GetProjectsByUserId(Guid userId);
        public Task<ActionResult> Create([FromBody] Project project);
        public Task<ActionResult<string>> GetCodeFromProject(Guid projectId);
        public Task<IActionResult> DeleteProject(Guid id);
        public Task<IActionResult> UpdateProjectTitle(Guid projectId, string newTitle);

    }
}
