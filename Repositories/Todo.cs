using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repositories
{
    public class Todo
    {
        public int NoteId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = null!;

        public string Description { get; set; }

        public DateTime ModifiedDate { get; set; }
        public DateTime Time { get; set; }
        public bool IsDone { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
