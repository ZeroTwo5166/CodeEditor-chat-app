using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class CodeForMonaco
    {
        private readonly ApplicationDbContext _context;
        public CodeForMonaco(ApplicationDbContext context)
        {
            _context = context;
        }


         public string GetCodeFromDatabase(Guid projectId)
        {
            // Implement the logic to fetch the "Code" field from the database
            // based on the project ID
            // You might use Entity Framework or any other data access method here
            // Adjust the logic based on your database schema and ORM
            // Return the fetched code
            return _context.Projects.Where(p => p.Id == projectId).Select(p => p.Code).FirstOrDefault();
        }

    }
}
