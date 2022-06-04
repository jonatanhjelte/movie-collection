using MovieCollection.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Services
{
    public interface IUserService
    {
        public Task<User?> AuthenticateAndGetUserAsync(string userName, string password);
        public Task CreateUserAsync(User user, string password);
    }
}
