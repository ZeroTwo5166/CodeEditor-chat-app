using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace backend.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
               _context = context;
        }

        //Creating Project
        public async Task<Guid> CreateProject(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project.Id;
        }

        //Deleting Project
        public async Task<bool> DeleteProject(Guid projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
            {
                return false; // Project not found
            }
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true; // Deleted successfully
        }
        
        //Get all projects
        public async Task<IEnumerable<Project>> GetAllProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        //Get code from project
        public async Task<string> GetCodeFromProject(Guid projectId)
        {
            if(projectId == null)
            {
                return "Enter correct ProjectId";
            }
            var project = await _context.Projects.FindAsync(projectId);
            if(project == null)
            {
                return "Project not found";
            }
            return project?.Code;
        }

        //Get project from projectId
        public async Task<Project> GetProjectById(Guid projectId)
        {
            return await _context.Projects.FindAsync(projectId);
        }

        //Get projects by userId
        public async Task<IEnumerable<Project>> GetProjectsByUserId(Guid userId)
        {
            return await _context.Projects.Where(p => p.UserId == userId).ToListAsync();
        }

        //Change projectTitle
        public async Task<bool> UpdateProjectTitle(Guid projectId, string newTitle)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
            {
                return false; // Project not found
            }
            project.ProjectName = newTitle;
            await _context.SaveChangesAsync();
            return true; // Updated successfully
        }
    }
}
