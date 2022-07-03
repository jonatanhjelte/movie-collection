using Bunit;
using MovieCollection.Tests.Helpers;
using MovieCollection.WebApp.Client.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MovieCollection.Tests.WebApp.Client.Tests
{
    public class HomePageTests : BaseTestPage
    {
        [Fact]
        public void IsNotLoggedIn_RedirectsToLogInPage()
        {
            var cut = RenderComponent<Home>();

            Assert.Contains("LOGIN", _mockNavMan.LastCalledUri.ToUpper());
        }

        [Fact]
        public void HasSearchMovieTextField()
        {
            _authContext.SetAuthorized("testUser");
            var cut = RenderComponent<Home>();

            var text = cut.Find("input#searchMovie");

            Assert.NotNull(text);
        }
        
        [Fact]
        public void ClickSearch_CallsHomePageModelSearch()
        {
            _authContext.SetAuthorized("testUser");
            var cut = RenderComponent<Home>();

            var text = cut.Find("input#searchMovie");
            text.TextContent = "testMovie";
        }
    }
}
