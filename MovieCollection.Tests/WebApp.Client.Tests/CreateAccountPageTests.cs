using Bunit;
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
    public class CreateAccountPageTests : BaseTestPage
    {
        [Fact]
        public void HasUserName()
        {
            var cut = RenderComponent<CreateAccount>();

            Assert.Contains("USERNAME", cut.Markup.ToUpper());
        }

        [Fact]
        public void HasEmail()
        {
            var cut = RenderComponent<CreateAccount>();

            Assert.Contains("EMAIL", cut.Markup.ToUpper());
        }

        [Fact]
        public void HasPassword()
        {
            var cut = RenderComponent<CreateAccount>();

            Assert.Contains("PASSWORD", cut.Markup.ToUpper());
        }

        [Fact]
        public void HasConfirmPassword()
        {
            var cut = RenderComponent<CreateAccount>();

            Assert.Contains("CONFIRM PASSWORD", cut.Markup.ToUpper());
        }

        [Fact]
        public void PasswordText_IsMasked()
        {
            var cut = RenderComponent<CreateAccount>();
            var passwordText = cut.Find("input#password");

            var type = passwordText.GetAttribute("type");

            Assert.Equal("password", type);
        }

        [Fact]
        public void PasswordConfirmText_IsMasked()
        {
            var cut = RenderComponent<CreateAccount>();
            var passwordText = cut.Find("input#passwordConfirm");

            var type = passwordText.GetAttribute("type");

            Assert.Equal("password", type);
        }
    }
}
