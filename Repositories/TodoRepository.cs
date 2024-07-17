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

		public List<Todo> GetAll()
		{
			_context = new TodoDbContext();
			return _context.Todos.ToList();
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

	}
}
