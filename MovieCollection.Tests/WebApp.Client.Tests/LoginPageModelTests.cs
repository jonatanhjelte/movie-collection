using Moq;
using MovieCollection.Tests.Helpers;
using MovieCollection.WebApp.Client.PageModels;
using MovieCollection.WebApp.Shared.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MovieCollection.Tests.WebApp.Client.Tests
{
    public class LoginPageModelTests
    {
        [Fact]
        public async Task Login_EmptyUserName_DoesNothing()
        {
            var pageModel = new LoginPageModel();
            var mockHandler = new MockHttpMessageHandler();
            pageModel.HttpClient = new HttpClient(mockHandler);

            await pageModel.Login(new LoginRequest() { UserName = string.Empty, Password = "123" });

            Assert.Empty(mockHandler.RequestMessages);
        }

        [Fact]
        public async Task Login_EmptyPassword_DoesNothing()
        {
            var pageModel = new LoginPageModel();
            var mockHandler = new MockHttpMessageHandler();
            pageModel.HttpClient = new HttpClient(mockHandler);

            await pageModel.Login(new LoginRequest() { UserName = "123", Password = string.Empty });

            Assert.Empty(mockHandler.RequestMessages);
        }

        [Fact]
        public async Task Login_UserNameAndPassword_SendsProperRequestAndRedirects()
        {
            var pageModel = new LoginPageModel();
            var mockNavMan = new MockNavigationManager();
            pageModel.NavigationManager = mockNavMan;
            var mockHandler = new MockHttpMessageHandler();
            pageModel.HttpClient = new HttpClient(mockHandler) { BaseAddress = new Uri(@"http://test.com") };

            await pageModel.Login(new LoginRequest() { UserName = "user123", Password = "password123" });

            Assert.Single(mockHandler.RequestMessages);
            var content = mockHandler.RequestMessages.First().Content;
            if (content == null)
            {
                throw new ArgumentNullException("No content in request.");
            }
            var text = await content.ReadAsStringAsync();
            Assert.Contains("user123", text);
            Assert.Contains("password123", text);
            Assert.Contains("HOME", mockNavMan.LastCalledUri.ToUpper());
        }

        [Fact]
        public async Task Login_UserNameAndPasswordGivesUnauthorized_AddsErrorToModel()
        {
            var pageModel = new LoginPageModel();
            var mockHandler = new MockHttpMessageHandler();
            mockHandler.DefaultResponse = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            pageModel.HttpClient = new HttpClient(mockHandler) { BaseAddress = new Uri(@"http://test.com") };

            await pageModel.Login(new LoginRequest() { UserName = "user123", Password = "password123" });

            Assert.Contains("INVALID USERNAME OR PASSWORD", pageModel.ErrorMessage.ToUpper());
        }

        [Fact]
        public void ClearErrorMessage_WhenCalled_ClearsErrorMessage()
        {
            var pageModel = new LoginPageModel();
            pageModel.ErrorMessage = "TEST";

            pageModel.ClearErrorMessage();

            Assert.Equal(string.Empty, pageModel.ErrorMessage);
        }
    }
}
