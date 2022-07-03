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

namespace MovieCollection.Tests.WebApp.Server.IntegrationTests
{
    public class MoviesControllerIntegrationTests : BaseControllerIntegrationTests
    {
        private readonly List<Movie> testMovies = new List<Movie>()
        {
            new Movie()
            {
                MovieDatabaseId = "t1234",
                Name = "testName1",
            },
            new Movie()
            {
                MovieDatabaseId = "t4321",
                Name = "testName2",
            },
        };

        [Fact]
        async Task Find_EmptyName_ReturnsBadRequestWithMessage()
        {
            var client = SetupTestAndGetClient();

            var response = await client.GetAsync(MovieRoute.FindMovie);
            var text = await response.Content.ReadAsStringAsync();

            Assert.False(response.IsSuccessStatusCode);
            Assert.Contains("NAME FIELD IS REQUIRED", text.ToUpper());
        }

        [Fact]
        async Task Find_NotLoggedIn_ReturnsUnauthorized()
        {
            var client = SetupTestAndGetClient();

            var response = await client.GetAsync(MovieRoute.FindMovie + "?name=1234");
            var text = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        async Task Find_NameLessThan4Characters_ReturnsBadRequestWithMessage()
        {
            var testUser = new User() { UserName = "TestUser" };
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(
                us => us.AuthenticateAndGetUserAsync(testUser.UserName, "testPassword").Result)
                        .Returns(testUser);
            var client = SetupTestAndGetClient(userServiceMock.Object);

            var _ = await client.PostAsJsonAsync(MovieRoute.LoginUser, new LoginRequest() { UserName = testUser.UserName, Password = "testPassword" });
            var response = await client.GetAsync(MovieRoute.FindMovie + "?name=123");
            var text = await response.Content.ReadAsStringAsync();

            Assert.False(response.IsSuccessStatusCode);
            Assert.Contains("MINIMUM LENGTH OF '4'", text.ToUpper());
        }

        [Fact]
        async Task Find_FindsMovies_ReturnsMovies()
        {
            var testUser = new User() { UserName = "TestUser" };
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(
                us => us.AuthenticateAndGetUserAsync(testUser.UserName, "testPassword").Result)
                        .Returns(testUser);
            var movieServiceMock = CreateMovieServiceMock();
            var client = SetupTestAndGetClient(userServiceMock.Object, null, movieServiceMock);

            var _ = await client.PostAsJsonAsync(MovieRoute.LoginUser, new LoginRequest() { UserName = testUser.UserName, Password = "testPassword" });
            var response = await client.GetAsync(MovieRoute.FindMovie + "?name=1234");
            var text = await response.Content.ReadAsStringAsync();

            var movies = JsonConvert.DeserializeObject<IEnumerable<Movie>>(text);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(testMovies, movies);
        }

        private IMovieService CreateMovieServiceMock()
        {
            var mock = new Mock<IMovieService>();
            mock.Setup(
                ms => ms.FindMoviesByNameAsync("1234").Result)
                    .Returns(testMovies);

            return mock.Object;
        }
    }
}
