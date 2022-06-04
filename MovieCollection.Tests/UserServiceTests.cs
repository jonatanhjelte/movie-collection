using MovieCollection.Domain;
using MovieCollection.Repositories.Abstractions;
using MovieCollection.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MovieCollection.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public async Task AuthenticateAndGetUserAsync_WrongUserName_ReturnsNull()
        {
            using var helper = new UserServiceTestHelper();

            var user = await helper.UserService.AuthenticateAndGetUserAsync("doesnotexist", "wrongpassword");

            Assert.Null(user);
        }
    }
}
