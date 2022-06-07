using MovieCollection.Tests.Helpers;
using MovieCollection.WebApp.Client.PageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MovieCollection.Tests.WebApp.Client.Tests
{
    public class BasePageModelTests
    {
        private MockNavigationManager _navMan;
        private HttpClient _httpClient;
        private MockHttpMessageHandler _mockMessageHandler;

        public BasePageModelTests()
        {
            _navMan = new MockNavigationManager();
            _mockMessageHandler = new MockHttpMessageHandler();
            _httpClient = new HttpClient(_mockMessageHandler) { BaseAddress = new Uri(@"http:\\test.com") };
        }

        [Fact]
        public async Task LogOut_SendsLogOutRequest()
        {
            var model = new BasePageModel();
            model.HttpClient = _httpClient;
            model.NavigationManager = _navMan;
            
            await model.Logout();

            Assert.Single(_mockMessageHandler.RequestMessages);
            Assert.Contains("LOGOUT", _mockMessageHandler.RequestMessages.First().RequestUri?.ToString().ToUpper());
        }

        [Fact]
        public async Task LogOut_RedirectsToLoginPage()
        {
            var model = new BasePageModel();
            model.HttpClient = _httpClient;
            model.NavigationManager = _navMan;

            await model.Logout();

            Assert.Contains("LOGIN", _navMan.LastCalledUri.ToUpper());
        }
    }
}
