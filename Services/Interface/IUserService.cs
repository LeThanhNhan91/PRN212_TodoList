using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        void AddUser(User user);
        void UpdateUser(User user);
        List<User> SearchUserByEmailAndFullName(string email, string fullName);
        void DeleteUser(User user);
        User Login(string email, string password);
    }
}
