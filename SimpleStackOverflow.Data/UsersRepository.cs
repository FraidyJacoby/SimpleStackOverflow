using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleStackOverflow.Data
{
    public class UsersRepository
    {
        private string _connectionString;

        public UsersRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public User LogIn(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }
            var validPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (validPassword)
            {
                return user;
            }
            return null;
        }

        public void AddUser(User user, string password)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            user.PasswordHash = passwordHash;

            using(var ctx = new QuestionsContext(_connectionString))
            {
                ctx.Users.Add(user);
                ctx.SaveChanges();
            }
        }

        public User GetByEmail(string email)
        {
            using (var ctx = new QuestionsContext(_connectionString))
            {
                return ctx.Users.FirstOrDefault(u => u.Email == email);
            }
        }
    }
}
