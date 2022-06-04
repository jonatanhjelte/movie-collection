using MovieCollection.Domain;
using MovieCollection.Domain.Exceptions;
using MovieCollection.Repositories;
using MovieCollection.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MovieCollection.Tests.ServicesTests
{
    public class UserServiceTests
    {
        private readonly User ExistingUser = new User()
        {
            UserName = "TestUserName",
        };

        private readonly User NotExistingUser = new User()
        {
            UserName = "NoExist",
        };

        private readonly string existingPassword = "123|Abc!^2";
        private readonly string notExistingPassword = "123|Abc!^2notexist";

        [Fact]
        public async Task AuthenticateAndGetUserAsync_WrongUserName_ReturnsNull()
        {
            using var helper = new UserServiceTestHelper();
            await InsertExistingUser(helper);

            var user = await helper.UserService.AuthenticateAndGetUserAsync(NotExistingUser.UserName, notExistingPassword);

            Assert.Null(user);
        }

        [Fact]
        public async Task AuthenticateAndGetUserAsync_CorrectUserNameWrongPassword_ReturnsNull()
        {
            using var helper = new UserServiceTestHelper();
            await InsertExistingUser(helper);

            var user = await helper.UserService.AuthenticateAndGetUserAsync(ExistingUser.UserName, existingPassword + "wrong");

            Assert.Null(user);
        }

        [Fact]
        public async Task AuthenticateAndGetUserAsync_CorrectUserNameAndPassword_ReturnsUser()
        {
            using var helper = new UserServiceTestHelper();
            await InsertExistingUser(helper);

            var user = await helper.UserService.AuthenticateAndGetUserAsync(ExistingUser.UserName, existingPassword);

            Assert.Equal(ExistingUser, user);
        }

        [Fact]
        public async Task AuthenticateAndGetUserAsync_UserNameWithDifferentCasing_ReturnsUser()
        {
            using var helper = new UserServiceTestHelper();
            await InsertExistingUser(helper);

            var user = await helper.UserService.AuthenticateAndGetUserAsync(ExistingUser.UserName.ToUpper(), existingPassword);
            var user2 = await helper.UserService.AuthenticateAndGetUserAsync(ExistingUser.UserName.ToLower(), existingPassword);


            Assert.Equal(ExistingUser, user);
            Assert.Equal(ExistingUser, user2);
        }

        [Fact]
        public async Task CreateUserAsync_CreateUser_CreatesUserWithEncryptedPassword()
        {
            using var helper = new UserServiceTestHelper();
            await InsertExistingUser(helper);

            var dbPassword = helper.Context.Users.Single(u => u.UserName == ExistingUser.UserName).PasswordHash;

            Assert.NotEqual(dbPassword, existingPassword);
        }

        [Fact]
        public async Task CreateUserAsync_UserAlreadyExists_ThrowUserExistsException()
        {
            using var helper = new UserServiceTestHelper();

            await InsertExistingUser(helper);

            await Assert.ThrowsAsync<UserAlreadyExistsException>(async () => await InsertExistingUser(helper));
        }

        private async Task InsertExistingUser(UserServiceTestHelper helper)
        {
            await helper.UserService.CreateUserAsync(ExistingUser, existingPassword);
        }
    }
}
