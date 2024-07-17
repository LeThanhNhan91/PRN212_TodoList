using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UserRepository
    {
        private TodoDbContext _context;

        public List<User> GetUser()
        {
            _context = new();
            return _context.Users.ToList();
        }

        public void Delete(User x)
        {
            _context = new();
            _context.Users.Remove(x);
            _context.SaveChanges();
        }
    }
}
