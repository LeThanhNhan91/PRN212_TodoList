using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace Repositories
{
	public class TodoRepository
	{
		private TodoDbContext _context;
       
        public TodoRepository()
		{
			_context = new TodoDbContext();
		}

		public List<Todo> GetAll()
		{
			_context = new TodoDbContext();
			return _context.Todos.ToList();
		}
        public Todo GetById(int id)
        {
            _context = new TodoDbContext();
            return _context.Todos.Where(m => m.NoteId == id).FirstOrDefault();
        }

        public void Add(Todo todo)
		{
			_context = new TodoDbContext();
			_context.Todos.Add(todo);
			_context.SaveChanges();

		}
		public void Update(Todo todo)
		{
			_context = new TodoDbContext();
			_context.Todos.Update(todo);
			_context.SaveChanges();
		}
		public void Remove(Todo todo)
		{
			_context = new TodoDbContext();
			_context.Remove(todo);
			_context.SaveChanges();
		}
        public void ShareTask(int todoId, int userId)
        {
            var sharedTask = new SharedTask
            {
                TodoId = todoId,
                UserId = userId
            };

            _context.SharedTasks.Add(sharedTask);
			
		
            _context.SaveChanges();
            
        }
    }
}
