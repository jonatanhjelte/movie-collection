using Microsoft.EntityFrameworkCore;
using MovieCollection.Domain;
using MovieCollection.Domain.Exceptions;
using MovieCollection.Repositories;
using MovieCollection.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Services
{
    public class UserService
    {
        private readonly MovieContext _context;

        public UserService(MovieContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateAndGetUserAsync(string userName, string password)
        {
            User? user = null;

            var userModel = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == userName);

            if (userModel != null
                && BCrypt.Net.BCrypt.Verify(password, userModel.PasswordHash))
            {
                user = userModel.ToDomainUser();
            }

            return user;
        }

        public async Task CreateUserAsync(User user, string password)
        {
            var existingUserModel = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);
            if (existingUserModel != null) throw new UserAlreadyExistsException();

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var userModel = new UserModel()
            {
                UserName = user.UserName,
                PasswordHash = hashedPassword,
                Id = -1,
            };

            _context.Users.Add(userModel);
            await _context.SaveChangesAsync();
        }
    }
}
