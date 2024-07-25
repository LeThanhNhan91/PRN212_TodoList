using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories
{
    public class Todo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public bool IsDone { get; set; }



        public virtual User User { get; set; } = null!;
        public virtual ICollection<SharedTask> SharedTasks { get; set; } = new List<SharedTask>();
    }
}
