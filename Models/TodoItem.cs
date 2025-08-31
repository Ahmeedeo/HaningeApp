using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaningeServer.Models
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public bool IsDone { get; set; }

        //Kopplay Todo till en användare
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; } 
    }
}
