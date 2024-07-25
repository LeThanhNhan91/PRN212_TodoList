using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class SharedTask
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SharedTaskId { get; set; }

        [Required]
        [ForeignKey("Todo")]
        public int TodoId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual Todo Todo { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
