using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using MovieCollection.Domain;
using MovieCollection.Domain.Exceptions;
using MovieCollection.Repositories;
using MovieCollection.Services;
using MovieCollection.WebApp.Shared.Requests;
using MovieCollection.WebApp.Shared.Routes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Xunit;

namespace MovieCollection.Tests.WebApp.Server.Tests
{
    public class UserControllerTests
    {
        [Fact]
        public async Task Login_UsernameEmpty_ReturnsBadRequestWithMessage()
        {
            var client = SetupTestAndGetClient(null);
            var content = BuildContent(new LoginRequest() { Password = "123" });
            
            var response = await client.PostAsync(MovieRoute.LoginUser, content);
            var text = await response.Content.ReadAsStringAsync();

            Assert.False(response.IsSuccessStatusCode);
            Assert.Contains("USERNAME FIELD IS REQUIRED", text.ToUpper());
        }

        [Fact]
        public async Task Login_PasswordEmpty_ReturnsBadRequestWithMessage()
        {
            var client = SetupTestAndGetClient(null);
            var content = BuildContent(new LoginRequest() { UserName = "123" });

            var response = await client.PostAsync(MovieRoute.LoginUser, content);
            var text = await response.Content.ReadAsStringAsync();

            Assert.False(response.IsSuccessStatusCode);
            Assert.Contains("PASSWORD FIELD IS REQUIRED", text.ToUpper());
        }

        [Fact]
        public async Task Login_InvalidUserNameOrPassword_ReturnsUnauthorized()
        {
            var client = SetupTestAndGetClient();
            var content = BuildContent(new LoginRequest() { UserName = "123", Password = "123" });

            var response = await client.PostAsync(MovieRoute.LoginUser, content);
            var text = await response.Content.ReadAsStringAsync();

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
            Assert.Contains("INVALID USERNAME OR PASSWORD", text.ToUpper());
        }

        [Fact]
        public async Task Login_CorrectUserNameAndPassword_ReturnsAndSignInUser()
        {
            var testUser = new User() { UserName = "testUser", Email = "testEmail" };
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(
                us => us.AuthenticateAndGetUserAsync(testUser.UserName, "testPassword").Result)
                        .Returns(testUser);

            var authenticationMock = new Mock<IAuthenticationService>();
            var client = SetupTestAndGetClient(userServiceMock.Object, authenticationMock.Object);
            var content = BuildContent(new LoginRequest() { UserName = testUser.UserName, Password = "testPassword" });

            var response = await client.PostAsync(MovieRoute.LoginUser, content);
            var text = await response.Content.ReadAsStringAsync();
            var returnedUser = JsonConvert.DeserializeObject<User>(text);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(testUser, returnedUser);
            // Verify SignInAsync was called at least once
            authenticationMock.Verify(
                m => m.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties?>()), Times.Once);
        }

        [Fact]
        public async Task Create_UserNameEmpty_ReturnsBadRequestWithMessage()
        {
            var request = new CreateUserRequest() { Password = "123", Email = "123" };
            var client = SetupTestAndGetClient();

            var response = await client.PostAsJsonAsync(MovieRoute.CreateUser, request);
            var text = await response.Content.ReadAsStringAsync();

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Contains("USERNAME FIELD IS REQUIRED", text.ToUpper());
        }

        [Fact]
        public async Task Create_PasswordEmpty_ReturnsBadRequestWithMessage()
        {
            var request = new CreateUserRequest() { UserName = "123", Email = "123" };
            var client = SetupTestAndGetClient();

            var response = await client.PostAsJsonAsync(MovieRoute.CreateUser, request);
            var text = await response.Content.ReadAsStringAsync();

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Contains("PASSWORD FIELD IS REQUIRED", text.ToUpper());
        }

        [Fact]
        public async Task Create_EmailEmpty_ReturnsBadRequestWithMessage()
        {
            var request = new CreateUserRequest() { UserName = "123", Password = "123" };
            var client = SetupTestAndGetClient();

            var response = await client.PostAsJsonAsync(MovieRoute.CreateUser, request);
            var text = await response.Content.ReadAsStringAsync();

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.Contains("EMAIL FIELD IS REQUIRED", text.ToUpper());
        }

        [Fact]
        public async Task Create_UserNameInUse_ReturnsConflictWithMessage()
        {
            var request = new CreateUserRequest() { UserName = "123", Password = "123", Email = "123" };
            var uService = new Mock<IUserService>();
                uService.Setup(us => us.CreateUserAsync(It.IsAny<User>(), It.IsAny<string>()))
                .Throws<UserAlreadyExistsException>();
            var client = SetupTestAndGetClient(uService.Object);

            var response = await client.PostAsJsonAsync(MovieRoute.CreateUser, request);
            var text = await response.Content.ReadAsStringAsync();

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Conflict);
            Assert.Contains("USERNAME ALREADY EXISTS", text.ToUpper());
        }

        [Fact]
        public async Task Create_CorrectInformation_CreatesAndReturnsUser()
        {
            var request = new CreateUserRequest() { UserName = "user", Password = "password", Email = "email" };
            var expectedUser = new User() { UserName = request.UserName, Email = request.Email };
            var uService = new Mock<IUserService>();
            var client = SetupTestAndGetClient(uService.Object);

            var response = await client.PostAsJsonAsync(MovieRoute.CreateUser, request);
            var user = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

            uService.Verify(us => us.CreateUserAsync(
                It.Is<User>(u => u == expectedUser),
                It.Is<string>(s => s == request.Password)));
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(expectedUser, user);
        }

        [Fact]
        public async Task GetCurrentUser_NotLoggedIn_ReturnsUnauthorized()
        {
            var client = SetupTestAndGetClient();

            var response = await client.GetAsync(MovieRoute.CurrentUser);

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetCurrentUser_LoggedIn_ReturnsCurrentUser()
        {
            var testUser = new User() { UserName = "TestUser" };
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(
                us => us.AuthenticateAndGetUserAsync(testUser.UserName, "testPassword").Result)
                        .Returns(testUser);
            var client = SetupTestAndGetClient(userServiceMock.Object, null, true);

            var _ = await client.PostAsJsonAsync(MovieRoute.LoginUser, new LoginRequest() { UserName = testUser.UserName, Password = "testPassword" });
            var response = await client.GetAsync(MovieRoute.CurrentUser);
            var text = await response.Content.ReadAsStringAsync();
            var returnedUser = JsonConvert.DeserializeObject<User>(text);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(testUser, returnedUser);
        }

        [Fact]
        public async Task Logout_NotLoggedIn_ReturnsUnauthorized()
        {
            var client = SetupTestAndGetClient();

            var response = await client.PostAsync(MovieRoute.LogoutUser, null);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Logout_IsLoggedIn_LogsOutUser()
        {
            var testUser = new User() { UserName = "TestUser" };
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(
                us => us.AuthenticateAndGetUserAsync(testUser.UserName, "testPassword").Result)
                        .Returns(testUser);
            var client = SetupTestAndGetClient(userServiceMock.Object, null, true);

            var _ = await client.PostAsJsonAsync(MovieRoute.LoginUser, new LoginRequest() { UserName = testUser.UserName, Password = "testPassword" });
            var logoutResponse = await client.PostAsync(MovieRoute.LogoutUser, null);
            var currentUserResponse = await client.GetAsync(MovieRoute.CurrentUser);

            Assert.True(logoutResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, currentUserResponse.StatusCode);
        }

        private HttpContent BuildContent(object jsonObj)
        {
            return new StringContent(JsonConvert.SerializeObject(jsonObj), Encoding.UTF8, "application/json");
        }

        private HttpClient SetupTestAndGetClient(
            IUserService? userServiceMock = null, 
            IAuthenticationService? authMock = null,
            bool skipAuthMock = false)
        {
            if (userServiceMock == null)
            {
                userServiceMock = new Mock<IUserService>().Object;
            }

            if (authMock == null)
            {
                authMock = new Mock<IAuthenticationService>().Object;
            }

            var application = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, configBuilder) =>
                    {
                        configBuilder.AddInMemoryCollection(
                            new Dictionary<string, string>
                            {
                                     {"ConnectionStrings:Database", $"Data Source=test.db"},
                            });
                    });

                    builder.ConfigureServices(services =>
                    {
                        services.Remove(services.Single(d => d.ServiceType == typeof(DbContextOptions<MovieContext>)));
                        services.Remove(services.Single(d => d.ServiceType == typeof(MovieContext)));

                        services.AddDbContext<MovieContext, FileMovieContext>();
                        services.AddScoped(p => userServiceMock);

                        if (!skipAuthMock)
                        {
                            services.AddSingleton(p => authMock);
                        }
                    });
                });
            
            var client = application.CreateClient();

            return client;
        }
    }
}
