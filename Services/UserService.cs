using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService
    {
        private UserRepository _repo = new();

        public List<User> GetAllUsers() 
        {
            return _repo.GetUser();
        }

        public void AddUser(User user)
        {
            _repo.Add(user);
        }

        public void DeleteUser(User x)
        {
            _repo.Delete(x);
        }
    }
}
