using MovieCollection.Domain;
using MovieCollection.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollection.Tests.Helpers
{
    internal class UserServiceTestHelper : BaseTestHelper
    {
        public readonly User ExistingUser;
        public readonly User NotExistingUser;
        public readonly UserService UserService;

        public UserServiceTestHelper()
        {
            ExistingUser = new User()
            {
                Id = 1,
                UserName = "TestUserName",
                Password = "TestPassword",
                Movies = new List<Movie>(),
            };

            NotExistingUser = new User()
            {
                Id = 1,
                UserName = "NoExist",
                Password = "NoExistPassword",
                Movies = new List<Movie>(),
            };

            MovieRepository.Users.Add(ExistingUser);
            MovieRepository.SaveChanges();

            UserService = new UserService(MovieRepository);
        }
    }
}
