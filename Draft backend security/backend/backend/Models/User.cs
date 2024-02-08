using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class User
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required]
        public byte[] Password { get; set; }
        public byte[] PasswordKey { get; set; }

        public byte[]? ProfilePic { get; set; }

        // Navigation property for projects
        public List<Project>? Projects { get; set; }
    }
}
