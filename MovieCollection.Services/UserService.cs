using MovieCollection.Domain;
using MovieCollection.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Services
{
    public class UserService
    {
        private readonly IMovieRepository _repo;

        public UserService(IMovieRepository repo)
        {
            _repo = repo;
        }

        public Task<User?> AuthenticateAndGetUserAsync(string userName, string password)
        {
            return Task.FromResult<User?>(null);
        }
    }
}
