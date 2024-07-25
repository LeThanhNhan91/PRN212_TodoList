using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Password { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        public int Role { get; set; }


        public virtual ICollection<Todo> Todos { get; set; } = new List<Todo>();
        public virtual ICollection<SharedTask> SharedTasks { get; set; } = new List<SharedTask>();
    }
}
