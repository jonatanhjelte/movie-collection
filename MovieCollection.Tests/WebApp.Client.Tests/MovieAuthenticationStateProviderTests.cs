using MovieCollection.Domain;
using MovieCollection.Tests.Helpers;
using MovieCollection.WebApp.Client.Providers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MovieCollection.Tests.WebApp.Client.Tests
{
    public class MovieAuthenticationStateProviderTests
    {
        [Fact]
        public async Task GetAuthenticationStateAsync_NotSuccessResponse_ReturnsEmptyAuthenticationState()
        {
            var mockHandler = new MockHttpMessageHandler();
            mockHandler.DefaultResponse = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            var client = new HttpClient(mockHandler) { BaseAddress = new Uri(@"http:\\test.com")};
            var provider = new MovieAuthenticationStateProvider(client);

            var result = await provider.GetAuthenticationStateAsync();

            Assert.Empty(result.User.Claims);
            Assert.False(result.User.Identity?.IsAuthenticated);
        }

        [Fact]
        public async Task GetAuthenticationStateAsync_IsLoggedIn_ReturnsLoggedInAuthenticationState()
        {
            var testUser = new User() { UserName = "testUser" };
            var mockHandler = new MockHttpMessageHandler();
            mockHandler.DefaultResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            mockHandler.DefaultResponse.Content = new StringContent(JsonConvert.SerializeObject(testUser), Encoding.UTF8, @"application/json");
            var client = new HttpClient(mockHandler) { BaseAddress = new Uri(@"http:\\test.com") };
            var provider = new MovieAuthenticationStateProvider(client);

            var result = await provider.GetAuthenticationStateAsync();

            Assert.NotEmpty(result.User.Claims);
            Assert.True(result.User.Identity?.IsAuthenticated);
        }
    }
}
