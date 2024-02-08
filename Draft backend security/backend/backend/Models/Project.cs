using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string ProjectName { get; set; }

        // Foreign key to User
        public Guid? UserId { get; set; }

        public string? Code { get; set; }

        // Navigation property to User
        public User? User { get; set; }
    }
}
