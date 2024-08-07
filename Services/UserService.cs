﻿using Repositories;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
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

        public void UpdateUser(User user)
        {
            _repo.Update(user);
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

        public User Login(string email, string password)
        {
            return _repo.GetUser().FirstOrDefault(u => u.Email ==  email && u.Password == password);
        }


    }
}
