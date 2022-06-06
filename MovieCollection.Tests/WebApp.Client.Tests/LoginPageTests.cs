using Bunit;
using Bunit.TestDoubles;
using MovieCollection.Tests.Helpers;
using MovieCollection.WebApp.Client.Components;
using MovieCollection.WebApp.Client.Pages;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MovieCollection.Tests.WebApp.Client.Tests
{
    public class LoginPageTests : BaseTestPage
    {
        [Fact]
        public void HasUserName()
        {
            var cut = RenderComponent<Login>();

            Assert.Contains("USERNAME", cut.Markup.ToUpper());
        }

        [Fact]
        public void HasPassword()
        {
            var cut = RenderComponent<Login>();

            Assert.Contains("PASSWORD", cut.Markup.ToUpper());
        }

        [Fact]
        public void HasLoginButton()
        {
            var cut = RenderComponent<Login>();

            Assert.Contains("LOGIN", cut.Markup.ToUpper());
        }

        [Fact]
        public void PasswordText_IsMasked()
        {
            var cut = RenderComponent<Login>();
            var passwordText = cut.Find("input#password");

            var type = passwordText.GetAttribute("type");

            Assert.Equal("password", type);
        }

        [Fact]
        public void HasCreateAccountButton()
        {
            var cut = RenderComponent<Login>();

            Assert.Contains("CREATE ACCOUNT", cut.Markup.ToUpper());
        }

        [Fact]
        public void CreateAccountLink_LinksToCreateAccountPage()
        {
            var cut = RenderComponent<Login>();
            var link = cut.Find("a");

            var href = link.GetAttribute("href");

            Assert.Equal("CreateAccount", href);
        }

        [Fact]
        public void ClickLogin_UserNameEmpty_ShowsError()
        {
            var cut = RenderComponent<Login>();
            var mudButton = cut.FindComponent<MudButton>();
            var btn = mudButton.Find("button");
            
            btn.Click();

            Assert.Contains("USERNAME IS REQUIRED", cut.Markup.ToUpper());
        }

        [Fact]
        public void ClickLogin_PasswordEmpty_ShowsError()
        {
            var cut = RenderComponent<Login>();
            var mudButton = cut.FindComponent<MudButton>();
            var btn = mudButton.Find("button");

            btn.Click();

            Assert.Contains("PASSWORD IS REQUIRED", cut.Markup.ToUpper());
        }

        [Fact]
        public void ErrorMessage_DisplaysOnPage()
        {
            var cut = RenderComponent<Login>();

            var errorMessage = "Invalid Username or Password";
            cut.Instance.ErrorMessage = errorMessage;
            cut.Render();

            Assert.Contains(errorMessage.ToUpper(), cut.Markup.ToUpper());
        }

        [Fact]
        public void ErrorMessageHasValue_SetErrorMessageToEmptyString_AfterUsernameOrPasswordChanged()
        {
            var cut = RenderComponent<Login>();

            var errorMessage = "Invalid Username or Password";
            cut.Instance.ErrorMessage = errorMessage;
            cut.Render();

            var input = cut.Find("input");
            input.Change("test");

            Assert.DoesNotContain(errorMessage.ToUpper(), cut.Markup.ToUpper());
        }

        [Fact]
        public void IsAlreadyLoggedIn_RedirectsToHome()
        {
            _authContext.SetAuthorized("testUser");
            
            var cut = RenderComponent<Login>();
            cut.Render();

            Assert.Contains("HOME", _mockNavMan.LastCalledUri.ToUpper());
        }
    }
}
