using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using MovieCollection.Domain;
using MovieCollection.Services;
using MovieCollection.WebApp.Shared.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Xunit;

namespace MovieCollection.Tests.WebApp.Server.Tests
{
    public class AuthenticationControllerTests
    {
        [Fact]
        public async Task Login_UsernameEmpty_ReturnsBadRequestWithMessage()
        {
            var client = SetupTestAndGetClient(null);
            var content = BuildContent(new LoginRequest() { Password = "123" });
            
            var response = await client.PostAsync("login", content);
            var text = await response.Content.ReadAsStringAsync();

            Assert.False(response.IsSuccessStatusCode);
            Assert.Contains("USERNAME FIELD IS REQUIRED", text.ToUpper());
        }

        [Fact]
        public async Task Login_PasswordEmpty_ReturnsBadRequestWithMessage()
        {
            var client = SetupTestAndGetClient(null);
            var content = BuildContent(new LoginRequest() { UserName = "123" });

            var response = await client.PostAsync("login", content);
            var text = await response.Content.ReadAsStringAsync();

            Assert.False(response.IsSuccessStatusCode);
            Assert.Contains("PASSWORD FIELD IS REQUIRED", text.ToUpper());
        }

        [Fact]
        public async Task Login_InvalidUserNameOrPassword_ReturnsUnauthorized()
        {
            var client = SetupTestAndGetClient();
            var content = BuildContent(new LoginRequest() { UserName = "123", Password = "123" });

            var response = await client.PostAsync("login", content);
            var text = await response.Content.ReadAsStringAsync();

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.Unauthorized);
            Assert.Contains("INVALID USERNAME OR PASSWORD", text.ToUpper());
        }

        [Fact]
        public async Task Login_CorrectUserNameAndPassword_ReturnsAndSignInUser()
        {
            var testUser = new User() { UserName = "testUser" };
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(
                us => us.AuthenticateAndGetUserAsync(testUser.UserName, "testPassword").Result)
                        .Returns(testUser);

            var authenticationMock = new Mock<IAuthenticationService>();
            var client = SetupTestAndGetClient(userServiceMock.Object, authenticationMock.Object);
            var content = BuildContent(new LoginRequest() { UserName = testUser.UserName, Password = "testPassword" });

            var response = await client.PostAsync("login", content);
            var text = await response.Content.ReadAsStringAsync();
            var returnedUser = JsonConvert.DeserializeObject<User>(text);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(testUser, returnedUser);
            // Verify SignInAsync was called at least once
            authenticationMock.Verify(
                m => m.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties?>()), Times.Once);
        }

        [Fact]
        public async Task GetCurrentUser_NotLoggedIn_ReturnsUnauthorized()
        {
            var client = SetupTestAndGetClient();

            var response = await client.GetAsync("currentuser");

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

            var _ = await client.PostAsJsonAsync("login", new LoginRequest() { UserName = testUser.UserName, Password = "testPassword" });
            var response = await client.GetAsync("currentuser");
            var text = await response.Content.ReadAsStringAsync();
            var returnedUser = JsonConvert.DeserializeObject<User>(text);

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(testUser, returnedUser);
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
                    builder.ConfigureServices(services =>
                    {
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
