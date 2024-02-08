using backend.Models;

namespace backend.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllProjects();
        Task<IEnumerable<Project>> GetProjectsByUserId(Guid userId);
        Task<Project> GetProjectById(Guid projectId);
        Task<Guid> CreateProject(Project project);
        Task<bool> UpdateProjectTitle(Guid projectId, string newTitle);
        Task<bool> DeleteProject(Guid projectId);
        Task<string> GetCodeFromProject(Guid projectId);
    }
}
