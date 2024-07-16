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

        //hàm search
        public List<User> SearchUserByEmailAndFullName(string email, string fullName)
        {
            email = email.ToLower();
            fullName = fullName.ToLower();

            return _repo.GetUser().Where(x => x.Email.ToLower().Contains(email) && x.FullName.ToLower().Contains(fullName)).ToList();
        }

        //hàm xóa
        public void DeleteUser(User x)
        {
            _repo.Delete(x);
        }
    }
}
