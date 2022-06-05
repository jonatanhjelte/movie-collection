using Bunit;
using MovieCollection.WebApp.Client.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MovieCollection.Tests.WebApp.Client.Tests
{
    public class LoginComponentTests : TestContext
    {
        [Fact]
        public void HasUserName()
        {
            var cut = RenderComponent<LoginComponent>();

            Assert.Contains("USERNAME", cut.Markup.ToUpper());
        }

        [Fact]
        public void HasPassword()
        {
            var cut = RenderComponent<LoginComponent>();

            Assert.Contains("PASSWORD", cut.Markup.ToUpper());
        }

        [Fact]
        public void HasLoginButton()
        {
            var cut = RenderComponent<LoginComponent>();

            Assert.Contains("LOGIN", cut.Markup.ToUpper());
        }

        [Fact]
        public void ClickLogin_UserNameEmpty_ShowsError()
        {
            var cut = RenderComponent<LoginComponent>();
            var mudButton = cut.FindComponent<MudButton>();
            var btn = mudButton.Find("button");
            
            btn.Click();

            Assert.Contains("USERNAME IS REQUIRED", cut.Markup.ToUpper());
        }

        [Fact]
        public void ClickLogin_PasswordEmpty_ShowsError()
        {
            var cut = RenderComponent<LoginComponent>();
            var mudButton = cut.FindComponent<MudButton>();
            var btn = mudButton.Find("button");

            btn.Click();

            Assert.Contains("PASSWORD IS REQUIRED", cut.Markup.ToUpper());
        }
    }
}
