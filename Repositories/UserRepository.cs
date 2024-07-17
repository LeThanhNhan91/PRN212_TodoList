﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Entities;

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

        public void Add(User user)
        {
            _context = new();
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context = new();
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(User x)
        {
            _context = new();
            _context.Users.Remove(x);
            _context.SaveChanges();
        }
    }
}
