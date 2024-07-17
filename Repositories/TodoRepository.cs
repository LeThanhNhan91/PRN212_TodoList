using Repositories.Entities;
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
            _context = new();
            return _context.Todos.ToList();
        }

        public void Add(Todo Todo)
        {
            _context = new();
            _context.Todos.Add(Todo);
            _context.SaveChanges();
        }

        public void Update(Todo Todo)
        {
            _context = new();
            _context.Todos.Update(Todo);
            _context.SaveChanges();
        }

        public void Delete(Todo x)
        {
            _context = new();
            _context.Todos.Remove(x);
            _context.SaveChanges();
        }
    }
}
