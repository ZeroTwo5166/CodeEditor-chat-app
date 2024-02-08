using backend.Models;

namespace backend.Repositories
{
    public class DelegateProjectRepository
    {

        /// <summary>
        /// Using generics delegate based repository pattern for this one controller.
        /// Just to show off my knowledge in advance C# topics.
        /// Why use delegates && generics??? => flexibility, cleaner and modular code, encapsulation of Repository Logic
        /// </summary>


        //DI
        private readonly ProjectRepository _repository;

        //Generic delegate => takes and return generic datas => which means one delegate can be used to reference many other methods with different parameter/return types.
        public delegate TResult ProjectDelegate<T, TResult>(T input);

        //CreateProjectDelegate 
        public ProjectDelegate<Project, Task<Guid>> CreateProjectDel;

        //DeleteProjectDelegate 
        public ProjectDelegate<Guid, Task<bool>> DeleteProjectDel;

        //GetAllProjects
        //public ProjectDelegate<{},Task<IEnumerable<Project>> 

        //Get code from project
        public ProjectDelegate<Guid, Task<string>> GetCodeFromProjectDel;

        //Get project(1) by projectId
        public ProjectDelegate<Guid, Task<Project>> GetProjectByIdDel;

        //Get PROJECTS by userid
        public ProjectDelegate<Guid, Task<IEnumerable<Project>>> GetProjectsByUserDel;

        // Combined delegate for GetAllProjects and UpdateProjectTitle
        public ProjectDelegate<object, Task<object>> GetAllProjectsAndUpdateTitleDel;


        public DelegateProjectRepository(ProjectRepository repository)
        {
            _repository = repository;

            // Assigning delegates to repository methods
            CreateProjectDel = _repository.CreateProject;
            DeleteProjectDel = _repository.DeleteProject;
            GetCodeFromProjectDel = _repository.GetCodeFromProject;
            GetProjectByIdDel = _repository.GetProjectById;
            GetProjectsByUserDel = _repository.GetProjectsByUserId;

            // Combined delegate to handle GetAllProjects and UpdateProjectTitle
            GetAllProjectsAndUpdateTitleDel = async (input) =>
            {
                // Check the type of input
                if (input is null)
                {
                    // Perform GetAllProjects if input is null
                    return await _repository.GetAllProjects();
                }
                else if (input is Tuple<Guid, string> parameters)
                {
                    // If input is Tuple<Guid, string>, perform UpdateProjectTitle
                    var projectId = parameters.Item1;
                    var newTitle = parameters.Item2;
                    return await _repository.UpdateProjectTitle(projectId, newTitle);
                }
                else
                {
                    // Handle invalid input
                    throw new ArgumentException("Invalid input type.");
                }
            };
        }
    }
}
